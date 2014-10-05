﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualRadar.Interface.Network;

namespace VirtualRadar.Library.Network
{
    /// <summary>
    /// The default implementation of <see cref="IPassphraseAuthentication"/>.
    /// </summary>
    /// <remarks><para>
    /// When the remote side connects it is expected to send the passphrase as a sequence
    /// of bytes in the following format:</para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Bytes</term>
    ///         <term>Type</term>
    ///         <term>Meaning</term>
    ///     </listheader>
    ///     <item>
    ///         <term>6</term>
    ///         <term>ASCII / UTF-8 characters</term>
    ///         <term>The magic string 'VRSPF1'</term>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <term>unsigned word, little-endian</term>
    ///         <term>Length of the passphrase.</term>
    ///     </item>
    ///     <item>
    ///         <term>Variable</term>
    ///         <term>UTF-8 encoded string</term>
    ///         <term>The passphrase. Cannot exceed 1024 bytes after encoding into UTF-8.</term>
    ///     </item>
    /// </list>
    /// </remarks>
    class PassphraseAuthentication : IPassphraseAuthentication
    {
        /// <summary>
        /// The magic number that the passphrase has to start with.
        /// </summary>
        public static readonly byte[] MagicNumber = Encoding.ASCII.GetBytes("VRSPF1");

        /// <summary>
        /// The largest passphrase (after encoded into UTF8) that we will allow.
        /// </summary>
        public static readonly int MaxPassphraseLength = 1024;

        /// <summary>
        /// See interface docs.
        /// </summary>
        public string Passphrase { get; set; }

        /// <summary>
        /// See interface docs.
        /// </summary>
        public int MaximumResponseLength
        {
            get { return MagicNumber.Length + 2 + MaxPassphraseLength; }
        }

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool GetResponseIsComplete(byte[] response)
        {
            var result = false;

            var headerLength = MagicNumber.Length + 2;
            if(response.Length >= headerLength) {
                var length = 0;
                if(HasMagicNumber(response))        length = GetPassphraseLength(response) + headerLength;
                else                                length = headerLength;
                if(length > MaximumResponseLength)  length = headerLength;

                if(length <= response.Length) result = true;
            }

            return result;
        }

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool GetResponseIsValid(byte[] response)
        {
            var result = false;

            var headerLength = MagicNumber.Length + 2;
            if(response.Length >= headerLength) {
                if(HasMagicNumber(response)) {
                    var passphraseLength = GetPassphraseLength(response);
                    if(passphraseLength <= MaxPassphraseLength) {
                        if(passphraseLength + headerLength <= response.Length) {
                            var passphrase = Encoding.UTF8.GetString(response, headerLength, passphraseLength);
                            result = passphrase == Passphrase;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <returns></returns>
        public byte[] SendAuthentication()
        {
            var passphrase = Encoding.UTF8.GetBytes(Passphrase);
            if(passphrase.Length > MaxPassphraseLength) throw new InvalidOperationException(String.Format("The passphrase is too long, it encodes to {0} bytes and the maximum is {1}", passphrase.Length, MaxPassphraseLength));

            var result = new byte[MagicNumber.Length + 2 + passphrase.Length];
            Array.Copy(MagicNumber, result, MagicNumber.Length);
            result[MagicNumber.Length + 0] = (byte)(passphrase.Length & 0xFF);
            result[MagicNumber.Length + 1] = (byte)((passphrase.Length & 0xFF00) >> 8);
            Array.ConstrainedCopy(passphrase, 0, result, MagicNumber.Length + 2, passphrase.Length);

            return result;
        }

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool HasMagicNumber(byte[] response)
        {
            var result = false;

            if(response.Length >= MagicNumber.Length) {
                result = true;
                for(var i = 0;i < MagicNumber.Length;++i) {
                    if(MagicNumber[i] != response[i]) {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the passphrase length encoded in the response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private int GetPassphraseLength(byte[] response)
        {
            int result = int.MaxValue;

            var offset = MagicNumber.Length;
            if(response.Length >= offset + 2) {
                var loByte = response[offset + 0];
                var hiByte = response[offset + 1];
                result = (hiByte << 8) + loByte;
            }

            return result;
        }
    }
}
