using System;
using System.Collections.Generic;
using System.IO;

namespace MegaLinker.Core
{
    internal sealed class LinkerOptions
    {
        #region Data Members

        public string OutputFile = "a.out";
        public HashSet<string> InputObjects = new HashSet<string>();

        #endregion

        #region Constructors

        private LinkerOptions() { }

        #endregion

        #region Operations

        public static LinkerOptions Parse(string[] args)
        {
            var linkerOptions = new LinkerOptions();

            var it = (args as IEnumerable<string>).GetEnumerator();
            while (it.MoveNext())
            {
                switch (it.Current)
                {
                    case "-o":
                        if (!it.MoveNext())
                        {
                            throw new IncompleteArgumentException("-o");
                        }
                        linkerOptions.OutputFile = it.Current;
                        break;

                    default:
                        var extension = Path.GetExtension(it.Current);

                        if (extension.ToLower() != ".mob")
                        {
                            throw new InvalidObjectExtensionException(it.Current);
                        }

                        linkerOptions.InputObjects.Add(it.Current);
                        break;
                }
            }

            if (linkerOptions.InputObjects.Count == 0)
            {
                throw new NoInputObjectsException();
            }

            return linkerOptions;
        }

        #endregion

        #region Fields
        #endregion
    }

    public class IncompleteArgumentException : Exception
    {
        #region Data Members

        private string message_ = null;

        #endregion

        #region Constructors

        public IncompleteArgumentException(string argumentName)
        {
            ArgumentName = argumentName;
        }

        #endregion

        #region Operations
        #endregion

        #region Fields

        public string ArgumentName { get; }

        public override string Message => message_ ?? (message_ = $"Incomplete Argument: {ArgumentName}");

        #endregion
    }

    public class NoInputObjectsException : Exception
    {
        #region Data Members
        #endregion

        #region Constructors
        #endregion

        #region Operations

        public override string Message => "No input objects were given";

        #endregion

        #region Fields
        #endregion
    }

    public class InvalidObjectExtensionException : Exception
    {
        #region Data Members

        private string message_ = null;

        #endregion

        #region Constructors

        public InvalidObjectExtensionException(string objectName)
        {
            ObjectName = objectName;
        }

        #endregion

        #region Operations

        public string ObjectName { get; }

        public override string Message => message_ ?? (message_ = $"Input object \"{ObjectName}\" has invalid extension. Expected .mob.");

        #endregion

        #region Fields
        #endregion
    }
}
