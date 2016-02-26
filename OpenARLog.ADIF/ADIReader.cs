/*
 * Copyright (C) 2016 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
 *
 * Author: kcotugno
 * Date: 2/23/2016
 */

using OpenARLog.ADIF;

using System;
using System.Collections.Generic;
using System.IO;


namespace OpenARLog.ADIF
{
    public class ADIReader
    {

        #region Private Members

        private string _filePath;

        private StreamReader _streamer;

        private string _headerText;

        // Lists to hold the data.
        private List<_Field> _headerFields;
        private List<_Field> _qsoFields;

        private struct _Field
        {
            public _FieldHeader _header;
            public string _data;
        }

        // A struct told hold an entry header to be passed to the data parser
        private struct _FieldHeader
        {
            public string name;
            public int length;
            public string type;
        }

        #endregion

        public ADIReader()
        {
            
        }
        
        public ADIReader(string filepath)
        {
            Open(filepath);
        }


        #region Public Functions

        public void Open(string filepath)
        {
            try
            {
                _streamer = new StreamReader(filepath, System.Text.Encoding.UTF8);
                _filePath = filepath;
                _headerText = string.Empty;

                _headerFields = new List<_Field>();
                _qsoFields = new List<_Field>();
            }
            catch
            {

            }
        }

        public void Close()
        {
            _streamer.Close();           
        }

        public void Read()
        {

            if (!_streamer.EndOfStream)
            {
                ParseHeader();
                ParseRecords();
            }

            //_headerFields.ForEach(x => Console.WriteLine(x._header.name + ":" + x._header.length + ":" + x._header.type + ":" + x._data));
            //_qsoFields.ForEach(x => Console.WriteLine(x._header.name + ":" + x._header.length + ":" + x._header.type + ":" + x._data));

        }

        #endregion


        #region Private Functions

        // Parse the file header. As per the format, if "<" is found at index 0, it is assumed
        // that there is no header and woudl thus move to scanning records. Otherwise the line is
        // passed to the line parser for further evaluation.
        private void ParseHeader()
        {
            bool isEOH = false;
            bool firstLine = true;

            string buffer = string.Empty;

            // Continue until there is no more header.
            while (!isEOH)
            {
                buffer = _streamer.ReadLine();

                // Make sure there is a header.
                if (firstLine && (buffer.IndexOf("<") == 0))
                    return;

                firstLine = false;

                // Parse the line.
                isEOH = ParseHeaderLine(buffer);
            }
        }

        private void ParseRecords()
        {
            bool isEOF = false;
            bool isEOR = false;

            string buffer = string.Empty;

            while (!isEOF)
            {
                buffer = _streamer.ReadLine();

                isEOR = ParseQSOLine(buffer);

                isEOF = _streamer.EndOfStream;
            }

        }

        
        // General line parser.
        private List<_Field> ParseLine(string line)
        {
            string sub = string.Empty;

            int index = 0;

            _Field record = new _Field();

            List<_Field> results = new List<_Field>();

            // Multiple fields can be on one line. Thus we must make sure all are accounted for.
            while (line.IndexOf("<", index) != -1 && line.IndexOf(">", index) != -1)
            {

                // Separate and pass just the header.
                record._header = ParseFieldHeader(line.Substring(index, (line.IndexOf(">", index) - index) + 1));

                // Separate and pass the field.
                record._data = GetFieldDataFromWithHeader(record._header, line.Substring(index, (line.IndexOf(">", index) - index) + record._header.length + 1));

                // Place record in a list in case there are multiple per line.
                results.Add(record);

                // Keep the index upto date on what has been parsed.
                index = line.IndexOf(">", index) + record._header.length + 1;
            }

            return results;
        }

        // File header parser. Calls on ParseLine(string).
        private bool ParseHeaderLine(string line)
        {

            // TODO throw an excpetion.
            // This should only be null if at the end of stream.
            // If a null line shows up here it means the file is corrupt.
            if (line == null)
                return true;

            int i = line.IndexOf("<");

            // If the line is from the header add the text to the header string.
            if (i == -1 && line != string.Empty)
            {
                _headerText += line + "\n";
            }
            else if (line != string.Empty)
            {

                _headerFields.AddRange(ParseLine(line));


                // This is a pretty inefficient way of finding the EOH. Fix later.
                // TODO Make more efficient.
                return _headerFields.Exists(x => x._header.name.ToUpper() == "EOH");

            }

            return false;
        }

        private bool ParseQSOLine(string line)
        {

            // TODO throw an excpetion.
            // This should only be null if at the end of stream.
            // If a null line shows up here it means the file is corrupt.
            if (line == null)
                return true;

            int i = line.IndexOf("<");

            // If the line is from the header add the text to the header string.
            if (line != string.Empty && i != -1)
            {
                _qsoFields.AddRange(ParseLine(line));
            }

            return false;
        }

        // Parse a field header,e.g., <programid:9>.
        private _FieldHeader ParseFieldHeader(string header)
        {
            // Struct to hold the header data.
            _FieldHeader results;
            results.name = string.Empty;
            results.length = 0;
            results.type = null;

            string sub = string.Empty;

            // These indexs will give us access to the data between the delimiters.
            int open = header.IndexOf("<");
            int first = header.IndexOf(":");
            int second = header.IndexOf(":", first + 1);
            int close = header.IndexOf(">");

            // The data name is simple and always in a header.
            if (first != -1)
            {
                results.name = header.Substring((open + 1), (first - open) - 1);
            }
            else
            {
                results.name = header.Substring((open + 1), (close - open) - 1);
                return results;
            }

            // Only user defined fields will have a third item in a header.
            // Currently, in the long run, user defined fields will be ignored.
            if (second == -1)
            {
                results.length = int.Parse(header.Substring((first + 1), (close - first) - 1));
            }
            else
            {
                results.length = int.Parse(header.Substring((first + 1), (second - first) - 1));

                results.type = header.Substring((second + 1), (close - second) - 1);
            }

            return results;

        }

        private string GetFieldDataFromWithHeader(_FieldHeader header, string line)
        {
            string data;

            data = line.Substring((line.IndexOf(">") + 1), header.length);

            return data;
        }

        #endregion
    }
}
