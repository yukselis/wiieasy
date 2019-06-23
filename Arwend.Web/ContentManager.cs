//using Microsoft.VisualBasic;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
////Imports Arwend.Application.Base
////Imports Arwend.Application.Framework
////Imports Arwend.View.Windows.Forms
////Imports System.Web
////Imports System.Windows.Forms
////Imports System.Drawing
////Namespace View.Web
////    Public Class ContentManager
////        Private sFileExtension As String = "ico"
////        Private sApplicationPath As String = ""
////        Private Shared DefaultNamespace As String = "Arwend"
////        Public Property RootNamespace() As String
////            Get
////                Return DefaultNamespace
////            End Get
////            Set(ByVal value As String)
////                DefaultNamespace = value
////            End Set
////        End Property
////        Public Property FileExtension() As String
////            Get
////                Return Me.sFileExtension
////            End Get
////            Set(ByVal value As String)
////                Me.sFileExtension = value
////            End Set
////        End Property
////        Friend ReadOnly Property ApplicationPath() As String
////            Get
////                Return Me.sApplicationPath
////            End Get
////        End Property
////        Public Function GetFile(ByVal FileName As String) As String
////            If FileName <> "" Then
////                Dim Parameters As String = ""
////                If FileName.IndexOf("?") > -1 Then
////                    Parameters = FileName.Substring(FileName.IndexOf("?") + 1)
////                    FileName = FileName.Substring(0, FileName.IndexOf("?"))
////                End If
////                Dim Value As String = Controls.ServerSide.CacheManager.GetCachedObject(FileName)
////                If Not Value Is Nothing Then
////                    Return Me.ReplaceParameters(Value, Parameters)
////                Else
////                    Dim FilePath As String = Me.ApplicationPath & FileName
////                    If System.IO.File.Exists(FilePath) Then
////                        Dim Reader As New System.IO.StreamReader(FilePath, System.Text.Encoding.GetEncoding(1254))
////                        Dim Content As String = Reader.ReadToEnd
////                        Controls.ServerSide.CacheManager.AddCache(FileName, Content, 1440)
////                        Reader.Close()
////                        Memory.Manager.Flush("Arwend_GetFile_2")
////                        Return Me.ReplaceParameters(Content, Parameters)
////                    Else
////                        Return ""
////                    End If
////                End If
////            Else
////                Return ""
////            End If
////        End Function
////        Public Function GetFile(ByVal [Namespace] As String, ByVal FileName As String, ByVal Extension As String, ByVal CahceFile As Boolean) As String
////            If [Namespace] = "" Then [Namespace] = Me.RootNamespace
////            If FileName <> "" Then
////                Dim Parameters As String = ""
////                Dim Value As String = Controls.ServerSide.CacheManager.GetCachedObject(FileName & "." & Extension)
////                If Not Value Is Nothing AndAlso Value <> "" Then
////                    Return Value
////                Else
////                    Dim Reader As System.IO.StreamReader
////                    Dim resourceAssem As Reflection.Assembly = Reflection.Assembly.Load([Namespace])
////                    Reader = New System.IO.StreamReader(resourceAssem.GetManifestResourceStream([Namespace] & "." & FileName & "." & Extension), System.Text.Encoding.GetEncoding(1254))
////                    Dim Content As String = Reader.ReadToEnd
////                    Controls.ServerSide.CacheManager.AddCache(FileName & "." & Extension, Content, 1440)
////                    Reader.Close()
////                    Memory.Manager.Flush("Arwend_GetFile")
////                    Return Content
////                End If
////            End If
////            Return ""
////        End Function
////        Private Function ReplaceParameters(ByVal Content As String, ByVal Parameters As String) As String
////            If Parameters <> "" Then
////                If Parameters.IndexOf("&") > -1 Then
////                    Dim ParameterArray As String() = Parameters.Split("&")
////                    Dim Parameter As String
////                    For Each Parameter In ParameterArray
////                        Content = Me.ReplaceParameter(Content, Parameter)
////                    Next
////                Else
////                    If Parameters.IndexOf("=") > -1 Then
////                        Content = Me.ReplaceParameter(Content, Parameters)
////                    End If
////                End If
////            End If
////            Return Content
////        End Function
////        Private Function ReplaceParameter(ByVal Content As String, ByVal Parameters As String) As String
////            Dim Parameter As String() = Parameters.Split("=")
////            Content = Content.Replace("<% = " & Parameter(0) & " %>", Parameter(1))
////            Return Content
////        End Function
////        Public Function GetImage(ByVal [Namespace] As String, ByVal FullName As String, ByVal CacheImage As Boolean) As Byte()
////            Dim NameAndExtension() As String = FullName.Split(".")
////            Dim Name As String = ""
////            Dim Extension As String = ""
////            If NameAndExtension.Length = 2 Then
////                Name = NameAndExtension(0)
////                Extension = NameAndExtension(1)
////            Else
////                Name = FullName
////                Extension = Me.FileExtension
////            End If
////            Return Me.GetImage([Namespace], Name, Extension, CacheImage)
////        End Function
////        Private Function AddImage(ByVal [Namespace] As String, ByVal Name As String, ByVal Extension As String, ByVal CacheImage As Boolean) As Byte()
////            Dim Image() As Byte = CreateImage([Namespace], Name, Extension)
////            If Not Image Is Nothing AndAlso Image.Length > 0 AndAlso CacheImage Then Controls.ServerSide.CacheManager.AddCache(Name, Image, 1440)
////            Memory.Manager.Flush("Arwend_AddImage")
////            Return Image
////        End Function
////        Public Function GetImage(ByVal [Namespace] As String, ByVal Name As String, ByVal Extension As String, ByVal CacheImage As Boolean) As Byte()
////            If Extension = "" Then Return Me.GetImage([Namespace], Name, CacheImage)
////            Dim Image() As Byte = Controls.ServerSide.CacheManager.GetCachedObject(Name)
////            If Image Is Nothing Then
////                Image = Me.AddImage([Namespace], Name, Extension, CacheImage)
////                Return Image
////            End If
////            Return Image
////        End Function
////        Public Shared Function CreateImage(ByVal [NameSpace] As String, ByVal Name As String, ByVal Extension As String) As Byte()
////            If [NameSpace] = "" Then [NameSpace] = DefaultNamespace
////            Dim resourceAssem As Reflection.Assembly = Reflection.Assembly.Load([NameSpace])
////            Return CreateImage(resourceAssem.GetManifestResourceStream([NameSpace] & "." & Name & "." & Extension))
////        End Function
////        Public Shared Function CreateImageStream(ByVal [NameSpace] As String, ByVal Name As String, ByVal Extension As String) As System.IO.Stream
////            If [NameSpace] = "" Then [NameSpace] = DefaultNamespace
////            Dim resourceAssem As Reflection.Assembly = Reflection.Assembly.Load([NameSpace])
////            If Extension = "" Then
////                Return resourceAssem.GetManifestResourceStream([NameSpace] & "." & Name)
////            End If
////            Return resourceAssem.GetManifestResourceStream([NameSpace] & "." & Name & "." & Extension)
////        End Function
////        Public Shared Function CreateImage(ByVal Stream As System.IO.Stream) As Byte()
////            Dim Data(0) As Byte
////            System.Array.Resize(Data, Stream.Length)
////            Stream.Read(Data, 0, Stream.Length)
////            Stream.Dispose()
////            Memory.Manager.Flush("Arwend_CreateImage")
////            Return Data
////        End Function
////        Public Sub New(ByVal ApplicationPath As String)
////            Me.sApplicationPath = ApplicationPath
////        End Sub
////    End Class
////End Namespace
//using Arwend.Application.Base;
//using Arwend.Application.Framework;
//using Arwend.View.Windows.Forms;
//using System.Web;
//using System.Windows.Forms;
//using System.Drawing;
//namespace View.Web
//{
//    public class ContentManager
//    {
//        //Private oData As New Hashtable
//        //Private oImageList As ImageList
//        //Private oImageTable As Hashtable
//        //Private oIconTable As Hashtable
//        private string sFileExtension = "ico";
//        private string sApplicationPath = "";
//        private static System.Reflection.Assembly aAssembly;
//        private static Hashtable Assemblies = new Hashtable();
//        private static string DefaultNamespace = "Arwend";
//        private static string sImagePath = "Images";
//        private static bool bCanBeRegisterAssembly = true;
//        public bool CanBeRegisterAssembly {
//            get { return bCanBeRegisterAssembly; }
//            set { bCanBeRegisterAssembly = value; }
//        }
//        public string ImagePath {
//            get { return sImagePath; }
//            set { sImagePath = value; }
//        }

//        public string RootNamespace {
//            get { return DefaultNamespace; }
//            set { DefaultNamespace = value; }
//        }
//        public string FileExtension {
//            get { return this.sFileExtension; }
//            set { this.sFileExtension = value; }
//        }
//        //Private ReadOnly Property Data() As Hashtable
//        //    Get
//        //        Return Me.oData
//        //    End Get
//        //End Property
//        internal string ApplicationPath {
//            get { return this.sApplicationPath; }
//        }
//        private static byte[] GetAssemblyForPath(string Path)
//        {
//            System.IO.FileStream fs = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
//            System.IO.BinaryReader TS = new System.IO.BinaryReader(fs);
//            byte[] poBytes = null;
//            poBytes = TS.ReadBytes(Convert.ToInt32(fs.Length));
//            TS.Close();
//            fs.Close();
//            fs.Dispose();
//            return poBytes;
//        }
//        private static void RegisterAssembly(string LoadingNamespace = "")
//        {
//            if (!bCanBeRegisterAssembly)
//                return;
//            string BaseDirectory = "";
//            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
//            if (!System.IO.Directory.Exists(BaseDirectory + "/" + sImagePath)) {
//                System.IO.Directory.CreateDirectory(BaseDirectory + "/" + sImagePath);
//            }
//            System.Reflection.Assembly TempAssembly = null;
//            byte[] Bytes = null;
//            string TempPath = null;
//            string Path = null;
//            string[] TempPaths = null;
//            for (int i = 0; i <= My.Application.Info.LoadedAssemblies.Count - 1; i++) {
//                try {
//                    TempAssembly = My.Application.Info.LoadedAssemblies(i);
//                    Path = TempAssembly.CodeBase;
//                    TempPaths = Path.Split("/");
//                    TempPath = TempPaths[TempPaths.Length - 1];
//                    TempPath = TempPath.Replace(".dll", ".images").Replace(".exe", ".images").Replace(".DLL", ".images").Replace(".EXE", ".images");
//                    Path = Path.Replace("/", "\\").Replace("file:\\\\\\", "");
//                    System.Type[] Types = TempAssembly.GetTypes();
//                    if (Types != null && Types.Length > 0 && !string.IsNullOrEmpty(Types[0].Namespace)) {
//                        string Namespace = Types[0].Namespace.Replace(".My", "");
//                        if (string.IsNullOrEmpty(LoadingNamespace) || LoadingNamespace == Namespace) {
//                            if (Path.Contains(AppDomain.CurrentDomain.BaseDirectory) && Assemblies[Namespace] == null) {
//                                try {
//                                    if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + sImagePath + "\\" + TempPath)) {
//                                        System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + sImagePath + "\\" + TempPath);
//                                    }
//                                    Bytes = GetAssemblyForPath(Path);
//                                    System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + sImagePath + "\\" + TempPath, Bytes);

//                                } catch (Exception ex) {
//                                }
//                                try {
//                                    TempAssembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(AppDomain.CurrentDomain.BaseDirectory + sImagePath + "\\" + TempPath);
//                                    Assemblies[Namespace] = TempAssembly;
//                                } catch (Exception ex) {
//                                    try {
//                                        TempPath = TempPath.Replace(".images", "");
//                                        VBMath.Randomize(1000000);
//                                        TempPath += VBMath.Rnd() + ".images";
//                                        System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + sImagePath + "\\" + TempPath, Bytes);
//                                        TempAssembly = System.Reflection.Assembly.ReflectionOnlyLoadFrom(AppDomain.CurrentDomain.BaseDirectory + sImagePath + "\\" + TempPath);
//                                        Assemblies[Namespace] = TempAssembly;

//                                    } catch (Exception ex2) {
//                                    }
//                                }
//                            }
//                        }
//                    }

//                } catch (Exception ex) {
//                } finally {
//                    TempAssembly = null;
//                    Bytes = null;
//                }
//            }
//        }
//        public static void Clear(string ImagePath)
//        {
//            string[] Files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + sImagePath);
//            foreach (string sFile in Files) {
//                if (sFile.EndsWith(".images")) {
//                    try {
//                        System.IO.File.Delete(sFile);

//                    } catch (Exception ex) {
//                    }
//                }
//            }
//        }
//        public string GetFile(string FileName)
//        {
//            if (!string.IsNullOrEmpty(FileName)) {
//                string Parameters = "";
//                if (FileName.IndexOf("?") > -1) {
//                    Parameters = FileName.Substring(FileName.IndexOf("?") + 1);
//                    FileName = FileName.Substring(0, FileName.IndexOf("?"));
//                }
//                //Dim Value As String = Me.Data(FileName)
//                string Value = Controls.ServerSide.CacheManager.GetCachedObject(FileName);
//                if ((Value != null)) {
//                    return this.ReplaceParameters(Value, Parameters);
//                } else {
//                    string FilePath = this.ApplicationPath + FileName;
//                    if (System.IO.File.Exists(FilePath)) {
//                        System.IO.StreamReader Reader = new System.IO.StreamReader(FilePath, System.Text.Encoding.GetEncoding(1254));
//                        string Content = Reader.ReadToEnd();
//                        Controls.ServerSide.CacheManager.AddCache(FileName, Content, 1440);
//                        //Me.Data.Add(FileName, Content)
//                        Reader.Close();
//                        return this.ReplaceParameters(Content, Parameters);
//                    } else {
//                        return "";
//                    }
//                }
//            } else {
//                return "";
//            }
//        }
//        private static System.Reflection.Assembly GetAssembly(string AssemblyName)
//        {
//            if (AssemblyName == "Arwend") {
//                return System.Reflection.Assembly.GetExecutingAssembly();
//            }
//            if (Assemblies[AssemblyName] == null) {
//                RegisterAssembly(AssemblyName);
//            }
//            return Assemblies[AssemblyName];
//        }
//        public string GetFile(string Namespace, string FileName, string Extension, bool CahceFile)
//        {
//            if (string.IsNullOrEmpty(Namespace))
//                Namespace = this.RootNamespace;
//            if (!string.IsNullOrEmpty(FileName)) {
//                string Parameters = "";
//                //Dim Value As String = Me.Data(FileName & "." & Extension)
//                string Value = Controls.ServerSide.CacheManager.GetCachedObject(FileName + "." + Extension);
//                if ((Value != null) && !string.IsNullOrEmpty(Value)) {
//                    return Value;
//                    //Me.ReplaceParameters(Value, Parameters)
//                } else {
//                    System.IO.StreamReader Reader = null;
//                    Reader = new System.IO.StreamReader(GetAssembly(Namespace).GetManifestResourceStream(Namespace + "." + FileName + "." + Extension), System.Text.Encoding.GetEncoding(1254));
//                    string Content = Reader.ReadToEnd();
//                    //Me.Data(FileName & "." & Extension) = Content
//                    if (!string.IsNullOrEmpty(Content)) {
//                        Controls.ServerSide.CacheManager.AddCache(FileName + "." + Extension, Content, 1440);
//                    }
//                    Reader.Close();
//                    Reader.Dispose();
//                    return Content;
//                    //Me.ReplaceParameters(Content, Parameters)
//                }
//            }
//            return "";
//        }
//        private string ReplaceParameters(string Content, string Parameters)
//        {
//            if (!string.IsNullOrEmpty(Parameters)) {
//                if (Parameters.IndexOf("&") > -1) {
//                    string[] ParameterArray = Parameters.Split("&");
//                    string Parameter = null;
//                    foreach (string Parameter_loopVariable in ParameterArray) {
//                        Parameter = Parameter_loopVariable;
//                        Content = this.ReplaceParameter(Content, Parameter);
//                    }
//                } else {
//                    if (Parameters.IndexOf("=") > -1) {
//                        Content = this.ReplaceParameter(Content, Parameters);
//                    }
//                }
//            }
//            return Content;
//        }
//        private string ReplaceParameter(string Content, string Parameters)
//        {
//            string[] Parameter = Parameters.Split("=");
//            Content = Content.Replace("<% = " + Parameter[0] + " %>", Parameter[1]);
//            return Content;
//        }
//        //Public ReadOnly Property ImageTable() As Hashtable
//        //    Get
//        //        Return Me.oImageTable
//        //    End Get
//        //End Property
//        //Public ReadOnly Property ImageList() As ImageList
//        //    Get
//        //        Return Me.oImageList
//        //    End Get
//        //End Property
//        //Private ReadOnly Property IconTable() As Hashtable
//        //    Get
//        //        Return Me.oIconTable
//        //    End Get
//        //End Property
//        public byte[] GetImage(string Namespace, string FullName, bool CacheImage)
//        {
//            string[] NameAndExtension = FullName.Split(".");
//            string Name = "";
//            string Extension = "";
//            if (NameAndExtension.Length == 2) {
//                Name = NameAndExtension[0];
//                Extension = NameAndExtension[1];
//            } else {
//                Name = FullName;
//                Extension = this.FileExtension;
//            }
//            return this.GetImage(Namespace, Name, Extension, CacheImage);
//        }
//        private byte[] AddImage(string Namespace, string Name, string Extension, bool CacheImage)
//        {
//            byte[] Image = CreateImage(Namespace, Name, Extension);
//            //If Not Image Is Nothing AndAlso Image.Length > 0 AndAlso CacheImage Then Me.ImageTable.Add(Name, Image)
//            if ((Image != null) && Image.Length > 0 && CacheImage)
//                Controls.ServerSide.CacheManager.AddCache(Name, Image, 1440);
//            return Image;
//        }
//        public byte[] GetImage(string Namespace, string Name, string Extension, bool CacheImage)
//        {
//            if (string.IsNullOrEmpty(Extension))
//                return this.GetImage(Namespace, Name, CacheImage);
//            byte[] Image = Controls.ServerSide.CacheManager.GetCachedObject(Name);
//            if (Image == null) {
//                Image = this.AddImage(Namespace, Name, Extension, CacheImage);
//                return Image;
//            }
//            return Image;
//        }
//        public static byte[] CreateImage(string NameSpace, string Name, string Extension)
//        {
//            if (string.IsNullOrEmpty(NameSpace))
//                NameSpace = DefaultNamespace;
//            return CreateImage(GetAssembly(NameSpace).GetManifestResourceStream(NameSpace + "." + Name + "." + Extension));
//        }
//        public static System.IO.Stream CreateImageStream(string NameSpace, string Name, string Extension)
//        {
//            if (string.IsNullOrEmpty(NameSpace))
//                NameSpace = DefaultNamespace;
//            if (string.IsNullOrEmpty(Extension)) {
//                return GetAssembly(NameSpace).GetManifestResourceStream(NameSpace + "." + Name);
//            }
//            return GetAssembly(NameSpace).GetManifestResourceStream(NameSpace + "." + Name + "." + Extension);
//        }
//        public static byte[] CreateImage(System.IO.Stream Stream)
//        {
//            byte[] Data = new byte[1];
//            System.Array.Resize(ref Data, Stream.Length);
//            Stream.Read(Data, 0, Stream.Length);
//            Stream.Dispose();
//            return Data;
//        }
//        public void Initilize()
//        {
//            RegisterAssembly();
//        }
//        public ContentManager(string ApplicationPath)
//        {
//            this.sApplicationPath = ApplicationPath;
//            //Me.oIconTable = New Hashtable
//            //Me.oImageTable = New Hashtable
//            //Me.oImageList = New ImageList
//        }
//    }
//}

