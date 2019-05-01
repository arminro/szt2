// based on https://stackoverflow.com/questions/18392538/securestring-to-byte-c-sharp

namespace Authentication
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Static class holding extension methods to help implementing security
    /// </summary>
    public static class Security_Helper
    {
        // source: https://stackoverflow.com/questions/18392538/securestring-to-byte-c-sharp/25190648#25190648

            /// <summary>
            /// Computes the hash of of the inoput secure string with function given as a parameter
            /// </summary>
            /// <typeparam name="T">The time of output</typeparam>
            /// <param name="src">The secure string source</param>
            /// <param name="encrypt">A function to encrypt the data</param>
            /// <returns>The encrypted information</returns>
        public static T Compute<T>(this SecureString src, Func<byte[], T> encrypt)
        {
            IntPtr bstr = IntPtr.Zero;
            byte[] workArray = null;

            // setting garbage collector to leave the byte[] alone so long as the operation takes place
            GCHandle handle = GCHandle.Alloc(workArray, GCHandleType.Pinned);
            try
            {
                /*** PLAINTEXT EXPOSURE BEGINS HERE ***/
                bstr = Marshal.SecureStringToBSTR(src);
                unsafe
                {
                    byte* bstrBytes = (byte*)bstr;
                    workArray = new byte[src.Length * 2];

                    for (int i = 0; i < workArray.Length; i++)
                    {
                        workArray[i] = *bstrBytes++;
                    }
                }

                // returns encripted byte[]
                return encrypt(workArray);
            }
            finally
            {
                if (workArray != null)
                {
                    for (int i = 0; i < workArray.Length; i++)
                    {
                        workArray[i] = 0;
                    }
                }

                handle.Free();
                if (bstr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr);
                }

                /*** PLAINTEXT EXPOSURE ENDS HERE ***/
            }
        }
    }
}
