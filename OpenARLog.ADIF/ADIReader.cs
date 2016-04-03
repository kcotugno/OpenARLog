/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 2/23/2016
 */

using OpenARLog.ADIF;

using System;
using System.Collections.Generic;
using System.IO;

using OpenARLog.Data;


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

        // A struct told hold an entry header to be passed to the data parser.
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
                // TODO
            }
        }

        public void Close()
        {
            _streamer.Close();           
        }

        public List<QSO> Read()
        {
            List<QSO> contacts;

            if (!_streamer.EndOfStream)
            {
                ParseHeader();
                ParseRecords();

                contacts = GetQSOsFromRecords();
            }

            //_headerFields.ForEach(x => Console.WriteLine(x._header.name + ":" + x._header.length + ":" + x._header.type + ":" + x._data));
            //_qsoFields.ForEach(x => Console.WriteLine(x._header.name + ":" + x._header.length + ":" + x._header.type + ":" + x._data));

            return null;
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
                return _headerFields.Exists(x => x._header.name == "EOH");

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
                results.name = header.Substring((open + 1), (first - open) - 1).ToUpper();
            }
            else
            {
                results.name = header.Substring((open + 1), (close - open) - 1).ToUpper();
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

        private List<QSO> GetQSOsFromRecords()
        {
            List<QSO> qsos = new List<QSO>();
            QSO temp = new QSO();


            foreach (_Field x in _qsoFields)
            {
                if (x._header.name == "EOR")
                {
                    qsos.Add(temp);
                    temp = new QSO();
                }
                else
                {
                    switch (x._header.name)
                    {

                        case "NAME":
                            temp.Name = x._data;
                            break;

                        case "CALL":
                            temp.Callsign = x._data;
                            break;

                        case "COUNTRY":
                            temp.Country = x._data;
                            break;

                        case "STATE":
                            temp.State = x._data;
                            break;

                        case "CNTY":
                            temp.County = x._data;
                            break;

                        case "QTH":
                            temp.City = x._data;
                            break;

                        case "GRIDSQUARE":
                            temp.GridSquare = x._data;
                            break;

                        case "FREQ":
                            temp.Frequency = x._data;
                            break;

                        case "BAND":
                            temp.Band = x._data;
                            break;

                        case "MODE":
                            temp.Mode = x._data;
                            break;

                        case "TIME_ON":
                            temp.DateTimeOn = MergeDateAndTime(temp.DateTimeOn, GetTimeFromRecord(x));
                            break;

                        case "QSO_DATE":
                            temp.DateTimeOn = MergeDateAndTime(temp.DateTimeOn, GetDateFromRecord(x));
                            break;

                        case "TIME_OFF":
                            temp.DateTimeOff = MergeDateAndTime(temp.DateTimeOff, GetTimeFromRecord(x));
                            break;

                        case "QSO_DATE_OFF":
                            temp.DateTimeOff = MergeDateAndTime(temp.DateTimeOff, GetDateFromRecord(x));
                            break;

                        default:
                            break;
                    }
                }
            }

            return qsos;
        }

        // The date and time get stored in different filed in file.We need to merge these so TimeOn/TimeOff store both.
        private DateTime? MergeDateAndTime(DateTime? current, DateTime? toMerge)
        {
            if (current == null)
                return toMerge;

            int year = 0;
            int month = 0;
            int day = 0;

            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            
            if (current.Value.ToString("yyyyMMdd") == "15000101")
            {
                year = Convert.ToInt32(toMerge.Value.ToString("yyyy"));
                month = Convert.ToInt32(toMerge.Value.ToString("%M"));
                day = Convert.ToInt32(toMerge.Value.ToString("%d"));

                hours = Convert.ToInt32(current.Value.ToString("%H"));
                minutes = Convert.ToInt32(current.Value.ToString("%m"));
                seconds = Convert.ToInt32(current.Value.ToString("%s"));

                return new DateTime(year, month, day, hours, minutes, seconds, DateTimeKind.Utc);
            }  else
            {
                year = Convert.ToInt32(current.Value.ToString("yyyy"));
                month = Convert.ToInt32(current.Value.ToString("%M"));
                day = Convert.ToInt32(current.Value.ToString("%d"));

                hours = Convert.ToInt32(toMerge.Value.ToString("%H"));
                minutes = Convert.ToInt32(toMerge.Value.ToString("%m"));
                seconds = Convert.ToInt32(toMerge.Value.ToString("%s"));

                return new DateTime(year, month, day, hours, minutes, seconds, DateTimeKind.Utc);
            }
        }

        // The ADIF file format stores its times in UTC. We will leave any conversion to the parent program.
        DateTime? GetTimeFromRecord(_Field record)
        {
            if (record._header.length != 6 && record._header.length != 4)
                return null;

            string temp = string.Empty;

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            hours = Convert.ToInt32(record._data.Substring(0, 2));
            minutes = Convert.ToInt32(record._data.Substring(2, 2));

            if (record._data.Length == 6)
                seconds = Convert.ToInt32(record._data.Substring(4, 2));

            // The time and date are stored separately. So we will set the date here to 1/1/1500.
            // This date is such so we can know if the date needs to be updated when merging the
            // date and time. 
            return new DateTime(1500, 1, 1, hours, minutes, seconds, DateTimeKind.Utc);
        }

        // The ADIF file format stores its date with respect to the UTC time.
        private DateTime? GetDateFromRecord(_Field record)
        {
            if (record._header.length != 8)
                return null;

            int year = 0;
            int month = 0;
            int day = 0;

            year = Convert.ToInt32(record._data.Substring(0, 4));
            month = Convert.ToInt32(record._data.Substring(4, 2));
            day = Convert.ToInt32(record._data.Substring(6, 2));

            return new DateTime(year, month, day);
        }

        #endregion
    }
}
