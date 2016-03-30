/*
 * Copyright (C) 2016 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
 *
 * Author: kcotugno
 * Date: 2/23/2016
 */

using System;
using System.Collections.Generic;
using System.IO;
using OpenARLog.Data;

namespace OpenARLog.ADIF
{
    public class ADIWriter
    {

        public string ADIF_VERSION { get; private set; }

        public string ProgramID { get; set; }
        public string ProgramVersion { get; set; }
        public DateTime? CreationTime {get; set;}

        public string InitialComment { get; set; }



        #region Private Members

        private static string _ADIF_VER = "3.0.4";

        private string _filePath;
        private StreamWriter _streamer;

        private bool fileexists = false;

        #endregion

        public ADIWriter()
        {
            _filePath = string.Empty;
            Initialize();
        }

        public ADIWriter(string filepath, bool append)
        {
            Initialize();

            Open(filepath, append);
        }

        private void Initialize()
        {
            ADIF_VERSION = _ADIF_VER;

            ProgramID = string.Empty;
            ProgramVersion = string.Empty;
            CreationTime = null;

            InitialComment = string.Empty;

            _streamer = null;
        }

        #region Public Functions

        public bool DidFilePreexist()
        {
            return fileexists;
        }

        public void Open(string filepath, bool append)
        {
            fileexists = File.Exists(filepath);

            try
            {
                _streamer = new StreamWriter(filepath, append, System.Text.Encoding.UTF8);
                _filePath = filepath;

            }
            catch
            {

            }
        }

        public void Close()
        {
            _streamer.Close();
        }

        public void WriteHeader()
        {
            _streamer.Write(InitialComment);

            _streamer.WriteLine();
            _streamer.WriteLine();

            _streamer.WriteLine(string.Format("<ADIF_VER:{0}>{1}", ADIF_VERSION.Length, ADIF_VERSION));

            if (!(ProgramID == string.Empty))
                _streamer.WriteLine(string.Format("<PROGRAMID:{0}>{1}", ProgramID.Length, ProgramID));

            if (!(ProgramVersion == string.Empty))
                _streamer.WriteLine(string.Format("<PROGRAMVERSION:{0}>{1}", ProgramVersion.Length, ProgramVersion));


            _streamer.WriteLine("<EOH>");
            _streamer.WriteLine();
            _streamer.WriteLine();
        }

        public void WriteQSO(QSO qso)
        {

            // TODO Remove the null check. After the QSO class is updated to initialize its string properties to empty we will not have to do the double check.

            // Operator info.
            if (!(qso.Callsign == string.Empty || qso.Callsign == null))
                _streamer.WriteLine(string.Format("<CALL:{0}>{1}", qso.Callsign.Length, qso.Callsign));

            if (!(qso.Name == string.Empty || qso.Name == null))
                _streamer.WriteLine(string.Format("<NAME:{0}>{1}", qso.Name.Length, qso.Name));

            // Location info.
            if (!(qso.Country == string.Empty || qso.Country == null))
                _streamer.WriteLine(string.Format("<COUNTRY:{0}>{1}", qso.Country.Length, qso.Country));

            if (!(qso.State == string.Empty || qso.State == null))
                _streamer.WriteLine(string.Format("<STATE:{0}>{1}", qso.State.Length, qso.State));

            if (!(qso.County == string.Empty || qso.County == null))
                _streamer.WriteLine(string.Format("<CNTY:{0}>{1}", qso.County.Length, qso.County));

            if (!(qso.City == string.Empty || qso.City == null))
                _streamer.WriteLine(string.Format("<QTH:{0}>{1}", qso.City.Length, qso.City));

            if (!(qso.GridSquare == string.Empty || qso.GridSquare == null))
                _streamer.WriteLine(string.Format("<GRIDSQUARE:{0}>{1}", qso.GridSquare.Length, qso.GridSquare));

            // Operation info.
            if (!(qso.Frequency == string.Empty || qso.Frequency == null))
                _streamer.WriteLine(string.Format("<FREQ:{0}>{1}", qso.Frequency.Length, qso.Frequency));

            if (!(qso.Band == string.Empty || qso.Band == null))
                _streamer.WriteLine(string.Format("<BAND:{0}>{1}", qso.Band.Length, qso.Band));

            if (!(qso.Mode == string.Empty || qso.Mode == null))
                _streamer.WriteLine(string.Format("<MODE:{0}>{1}", qso.Mode.Length, qso.Mode));

            // TODO Add the rest of the fields.

            _streamer.WriteLine("<EOR>");
            _streamer.WriteLine();
        }

        public void WriteQSOList(List<QSO> qsos)
        {
            qsos.ForEach(x => WriteQSO(x));
        }

        #endregion
    }
}
