
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using UnityEngine;
using System.Diagnostics;

namespace TechninierException
{
    public sealed class ExceptionUtils
    {
        static ReaderWriterLockSlim rw = new ReaderWriterLockSlim();

        StringBuilder builderforWriteExceptionText = new StringBuilder();
        StringBuilder builder = new StringBuilder();
        //string SDCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/OffBDriver";

        static ExceptionUtils utils;

        public static ExceptionUtils Instance
        {
            get
            {
                if (utils == null)
                    utils = new ExceptionUtils();
                return utils;
            }
        }
        #region LOGGING ERROR MESSAGE
        public void GetLineNumber([CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int callingFileLineNumber = 0)
        {
            UnityEngine.Debug.Log("--------------------- File name --> " + callingFilePath + " ^^^^^ : Calling Method ---> " + callingMethod + "  ######## :-->>>  at Line No. : " + callingFileLineNumber);
        }




        /// <summary>
        /// Logs and print error message.
        /// </summary>
        /// <param name="exx">Exx.</param>
        /// <param name="list">List.</param>
        /// <param name="callingMethod">Calling method.</param>
        /// <param name="callingFilePath">Calling file path.</param>
        /// <param name="callingFileLineNumber">Calling file line number.</param>

        SynchronizationContext SyncContext = null;

        public void LogPrintErrorMessage(Exception exx, System.Threading.SynchronizationContext sCtx=null, List<string> list = null, [CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int callingFileLineNumber = 0, bool isLogFile = true)
        {
            SyncContext = sCtx;
            Dictionary<string, string> dictionaryMap = new Dictionary<string, string>();

            System.Runtime.ExceptionServices.ExceptionDispatchInfo ex = null;
            string sdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
            string strdatetime = $" {sdatetime} :";
            string datetime = strdatetime;
            try
            {
                ex = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exx);
                builderforWriteExceptionText.Length = 0;
                builder.Length = 0;
                builder.AppendFormat("{0}", "\r\n\r\n");
                builder.AppendFormat("{0}", "Beginning Exception Report ==================================================================================================================================================================================================================");
                builder.AppendFormat("{0}", "\r\n");


                builderforWriteExceptionText.AppendFormat("[{0}] >>>> [LOG_PRINT_ERROR_MESSAGE_Log.ErrorD] !!!!!!!!!!! ---> at [ Line No. : {1} ] : [ Calling Method --> {2} ] [ File name --> {3}]", datetime, callingFileLineNumber, callingMethod, callingFilePath);

                builderforWriteExceptionText.Append(ex.SourceException.Message + "|");

                dictionaryMap.Add("Log.Errord line number >>>>>>> :", string.Format("[LOG_PRINT_ERROR_MESSAGE_Log.ErrorD] !!!!!!!!!!! ---> at [ Line No. : {1} ] : [ Calling Method --> {2} ] [ File name --> {3}]", datetime, callingFileLineNumber, callingMethod, callingFilePath));


                builder.AppendFormat("[{0}] >>>> [LOG_PRINT_ERROR_MESSAGE_Log.ErrorD !!!!!!!!!!! ---> at [ Line No. : {1} ] : [ Calling Method --> {2} ] [ File name --> {3}]", datetime, callingFileLineNumber, callingMethod, callingMethod);
                builder.AppendFormat("{0}", "\r\n");

                if (list != null)
                {
                    string strlist = string.Join("\n", list.ToArray());
                    builder.AppendFormat("&&& went thru these line numbers---- {0}  &&&&&&&&&&&&&&&&&&&&&&&&", strlist);
                    builderforWriteExceptionText.AppendFormat("&&& went thru these line numbers----  {0}  &&&&&&&&&&&&&&&&&&&&&&&& ", strlist);
                    builderforWriteExceptionText.AppendFormat("{0}", "\r\n");
                }


                var stOuter = new StackTrace(ex.SourceException, true);
                int frameCountOuter = stOuter.FrameCount;
                //builder.AppendFormat ("[ \n {0} \n>> [Stack Frame count : --->>] --> {1}\n", strdatetime, frameCountOuter.ToString ());
                builder.AppendFormat("[ \n {0} \n >>>>>>>>>>>>>>>>>>>>>>>>>>>>\n [Error Message: --->>] --> {1} \n", strdatetime, ex.SourceException.Message);
                builder.AppendFormat("[ \n {0} \n>> [[Error Targetsite Something has gone horribly wrong with the method : --->> --> {1}\n", strdatetime, ex.SourceException.TargetSite);
                builder.AppendFormat("[ \n {0} \n>> [Error Source: --->>] --> {1}\n", strdatetime, ex.SourceException.Source);
                builder.AppendFormat("[ \n {0} \n >>>>>>>>>>>>>>>>>>>>>>>>>>>>\n [Error StackTrace: --->>] --> {1}\n", strdatetime, ex.SourceException.StackTrace);

                builderforWriteExceptionText.AppendFormat("[ \n {0} \n>> [Stack Frame count : --->>] --> {1}\n", strdatetime, frameCountOuter.ToString());
                builderforWriteExceptionText.AppendFormat("[ \n {0} \n>> [Error Message: --->>] --> {1} \n", strdatetime, ex.SourceException.Message);
                builderforWriteExceptionText.AppendFormat("[ \n {0} \n>> [Error Targetsite Something has gone horribly wrong with the method : --->>] --> {1}\n", strdatetime, ex.SourceException.TargetSite);
                builderforWriteExceptionText.AppendFormat("[ \n {0} \n>> [Error Source: --->>] --> {1}\n", strdatetime, ex.SourceException.Source);
                builderforWriteExceptionText.AppendFormat("[ \n {0} \n>> [Error StackTrace: --->>] --> {1}\n", strdatetime, ex.SourceException.StackTrace);

                var prevfilename = "";
                for (int i = 0; i < frameCountOuter; i++)
                {
                    StackFrame frameOuter = stOuter.GetFrame(i);

                    var filename = frameOuter.GetFileName();

                    var methodnamer = frameOuter.GetMethod();
                    string methodname = string.Empty;
                    if (methodnamer != null)
                        methodname = frameOuter.GetMethod().Name;
                    var iline = frameOuter.GetFileLineNumber();
                    var line = Convert.ToString(iline);
                    if (!string.IsNullOrEmpty(filename))
                    {
                        if (prevfilename != filename)
                        {

                            builder.AppendFormat("\n >>>>>>>>>>>>>>>>>>>>>>>>>>>>\n [Exception Caught at Caught at ] ->> [At LineNumber: {1}] >> [MethodName >: {2}] >> [FileName: {3}]", strdatetime, line, methodname, filename);
                            builderforWriteExceptionText.AppendFormat("\n >>>>>>>>>>>>>>>>>>>>>>>>>[\n  :][Exception Caught at Caught at ] ->> [At LineNumber: {1}] >> [MethodName >: {2}] >> [FileName: {3}]", strdatetime, line, methodname, filename);
                            dictionaryMap.Add(">>>>> " + i.ToString() + " : ", string.Format("[Exception Caught at Caught at ] ->> [At LineNumber: {1}] >> [MethodName >: {2}] >> [FileName: {3}]", strdatetime, line, methodname, filename));
                            prevfilename = filename;

                        }
                    }
                }
                builder.Append("\n >>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
                builderforWriteExceptionText.Append("\n >>>>>>>>>>>>>>>>>>>>>>>>>\n");
                if (ex.SourceException.InnerException != null)
                {
                    var sttinner = new StackTrace(ex.SourceException.InnerException, true);
                    int frameCountin = sttinner.FrameCount;
                    //builder.AppendFormat (" \n {0} \n>> [Inner Stack Frame count : --->>] --> {1}\n", strdatetime, frameCountin.ToString ());
                    builder.AppendFormat(" \n {0} \n>> [Inner Exception Message: --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.Message);
                    builder.AppendFormat(" \n {0} \n>> [Inner Exception Targetsite2 Something has gone horribly wrong with the method : --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.TargetSite);
                    builder.AppendFormat(" \n {0} \n>> [Inner Exception Source: --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.Source);
                    builder.AppendFormat(" \n {0} \n>> [Inner Exception StackTrace: --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.StackTrace);

                    builderforWriteExceptionText.AppendFormat(" \n {0} \n>> [Inner Stack Frame count : --->>] --> {1}\n", strdatetime, frameCountin.ToString());
                    builderforWriteExceptionText.AppendFormat(" \n {0} \n>> [Inner Exception Message: --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.Message);
                    builderforWriteExceptionText.AppendFormat(" \n {0} \n>> [Inner Exception Targetsite2 Something has gone horribly wrong with the method : --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.TargetSite);
                    builderforWriteExceptionText.AppendFormat(" \n {0} \n>> [Inner Exception Source: --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.Source);
                    builderforWriteExceptionText.AppendFormat(" \n {0} \n>> [Inner Exception StackTrace: --->>] --> {1} \n", strdatetime, ex.SourceException.InnerException.StackTrace);

                    var prevfile = "";
                    for (int ii = 0; ii < frameCountin; ii++)
                    {
                        StackFrame framein = sttinner.GetFrame(ii);

                        var filename = framein.GetFileName();
                        var methodname = framein.GetMethod().Name;
                        var iline = framein.GetFileLineNumber();
                        var line = Convert.ToString(iline);

                        if (!string.IsNullOrEmpty(filename))
                        {
                            if (prevfile != filename)
                            {

                                builder.AppendFormat("\n >>>\n {0} [Inner Inner Exception Caught at ] ->> [At LineNumber: {1}] >> [MethodName >: {2}] >> [FileName: {3}]", strdatetime, line, methodname, filename);
                                builderforWriteExceptionText.AppendFormat("\n >>>[\n {0} :][Inner Inner Exception Caught at ] ->> [At LineNumber: {1}] >> [MethodName >: {2}] >> [FileName: {3}]", strdatetime, line, methodname, filename);
                                //dictionaryMap.Add (Guid.NewGuid()  +" : Inner inner Exception Caught at Caught at :", line);
                                dictionaryMap.Add(">>>>>>>>> " + ii.ToString() + " ::", string.Format("[Inner Inner Exception Caught at Caught at ] ->> [At LineNumber: {1}] >> [MethodName >: {2}] >> [FileName: {3}]", strdatetime, line, methodname, filename));
                                prevfile = filename;

                            }
                        }
                    }
                }

                builder.Append("\n[\n");
                builder.AppendFormat("\n {0} :>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> [LOG_PRINT_ERROR_MESSAGE ENDS] !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!]", strdatetime);
                builder.Append("\n[\n");
                builder.Append("Completed Exception Report ================================================================================================================================================================================================\r\n\r\n");

                builderforWriteExceptionText.AppendFormat("\n[{0}] >>>> [LOG_PRINT_ERROR_MESSAGE ENDS] !!!!!!!!!!!\n", strdatetime);

                UnityEngine.Debug.LogError(builder.ToString());
                //Console.WriteLine(builder.ToString());
                //log the error to the file
                if (isLogFile == true)
                {
                    SyncContext.Post(_ =>
                    {
                        LogExceptionText(builderforWriteExceptionText.ToString());
                    }, null);
                   
                }

            }
            catch (Exception egc)
            {
                UnityEngine.Debug.LogError("----MMM----------MMMMM---------  "+egc.Message);
            }

        }



        /// <summary>
        /// Log the text to the file. Append the text to same file
        /// if same date
        /// </summary>
        /// <param name="strlog">Strlog.</param>
        /// <param name="isZipping">If set to <c>true</c> is zipping.</param>
        public void LogExceptionText(string strlog = "", bool isZipping = false)
        {

            try
            {
                bool t = true;
                TextWriter tw;

                var FolderPath = DirectoryPath();
                var AppName = ProductName();
                var AppVersion = Version();
                var fileName = string.Format("{0}_{1}_{2}.txt", AppName, AppVersion, DateTime.Now.ToString("yyyy-MM-dd"));
                var filepath = System.IO.Path.Combine(FolderPath, fileName);
                var fInfo = new FileInfo(filepath);

                rw.EnterWriteLock();

                if (!File.Exists(filepath))
                    t = false;

                using (tw = TextWriter.Synchronized(File.AppendText(filepath)))
                {
                    if (!t)
                        tw.Write(strlog);
                    else
                        tw.Write("^\n#####################################################################\\n" + strlog);

                    tw.Flush();
                    tw.Close();
                }

                rw.ExitWriteLock();
            }
            catch (System.Exception ex)
            {
                LogPrintErrorMessage(ex, null, null,"", "", 0, false);
            }
        }

        #endregion

      

        string DirectoryPath()
        {
            var path = string.Empty;
            SyncContext.Post(_ =>
            {
               path= Application.persistentDataPath;
            }, null);
            return path;
        }

        string ProductName()
        {
            var prod = string.Empty;
            SyncContext.Post(_ =>
            {
                prod = Application.productName;
            }, null);
            return prod;
        }

        string Version()
        {
            var version= string.Empty;
            SyncContext.Post(_ =>
            {
                version = Application.version;
            }, null);
            return version;
        }
    }
}

