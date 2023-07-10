using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TripBookingLibrary
{
    public class Encryption
    {
        // Encryption Key
        private static byte[] key = { 0x7d, 0x6d, 0x31, 0x36, 0xee, 0xac, 0x1b, 0x6f };

        // Permutation Tables for Plaintext or Ciphertext
        private static readonly int[] IP_TABLE =
        {
            58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17,  9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7
        };
        private static readonly int[] P_TABLE =
        {
            16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25
        };
        private static readonly int[] FP_TABLE =
        {
            40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25
        };

        // S-Boxes
        private static readonly int[,] S1 =
        {
            { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
            { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
            { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
            { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
        };
        private static readonly int[,] S2 =
        {
            { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
            { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
            { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
            { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
        };
        private static readonly int[,] S3 =
        {
            { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
            { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
            { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
            { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
        };
        private static readonly int[,] S4 =
        {
            { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
            { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
            { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
            { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
        };
        private static readonly int[,] S5 =
        {
            { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
            { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
            { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
            { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
        };
        private static readonly int[,] S6 =
        {
            { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
            { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
            { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
            { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
        };
        private static readonly int[,] S7 =
        {
            { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
            { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
            { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
            { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
        };
        private static readonly int[,] S8 =
        {
            { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
            { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
            { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
            { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
        };

        // Permutation Tables for Key Generation
        private static readonly int[] PC_1 =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };
        private static readonly int[] PC_2 =
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };

        /// <summary>
        /// Encrypts a string utilizing DES and returns the ciphertext.
        /// </summary>
        /// <param name="plainText"> the string to encrypt </param>
        /// <returns> the ciphertext </returns>
        public static string Encrypt(string plainText)
        {
            // Convert plaintext bytes with UTF-8 encoding.
            byte[] plainBytes;
            try
            {
                plainBytes = Encoding.UTF8.GetBytes(plainText);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            // Divide plaintext into 8-byte blocks.
            // If there are not enough bytes for a full 8-byte block at the end, then add padding bytes with the last
            // padding byte being the hex representation of how many padding bytes are required and the rest being 0x00.
            List<List<byte>> byteBlocks = new List<List<byte>>();
            List<byte> byteBlock_8 = new List<byte>();
            int paddingBytes = 0;
            for (int i = 0; i < plainBytes.Length; i++)
            {
                byteBlock_8.Add(plainBytes[i]);

                if ((i + 1) % 8 == 0)
                {
                    byteBlocks.Add(byteBlock_8);
                    byteBlock_8 = new List<byte>();
                }
            }
            if (byteBlock_8.Count != 0)
            {
                while (byteBlock_8.Count % 7 != 0)
                {
                    byteBlock_8.Add(0x00);
                    paddingBytes++;
                }
                paddingBytes++;

                byteBlock_8.Add(Convert.ToByte(paddingBytes));
                byteBlocks.Add(byteBlock_8);
            }

            // Encrypt each 8-byte block of plaintext.
            for (int i = 0; i < byteBlocks.Count; i++)
            {
                byteBlocks[i] = PassBytesIntoDES_Structure(byteBlocks[i].ToArray(), true).ToList<byte>();
            }

            // Combine all encrypted 8-byte blocks.
            List<byte> encryptedBytes = new List<byte>();
            foreach (List<byte> block in byteBlocks)
            {
                encryptedBytes.AddRange(block);
            }
            byte[] cipherBytes = encryptedBytes.ToArray();

            // Convert encrypted bytes to ciphertext with Base64 encoding.
            string cipherText;
            try
            {
                cipherText = Convert.ToBase64String(cipherBytes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return cipherText;
        }

        /// <summary>
        /// Decrypts a string utilizing DES and returns the plaintext.
        /// </summary>
        /// <param name="cipherText"> the string to decrypt </param>
        /// <returns> the plaintext </returns>
        public static string Decrypt(string cipherText)
        {
            // Convert ciphertext to bytes with Base64 encoding.
            byte[] cipherBytes;
            try
            {
                cipherBytes = Convert.FromBase64String(cipherText);
            }
            catch (Exception ex)
            {
                return "The input is not a properly encrypted string.";
            }

            // Divide ciphertext into 8 byte-blocks.
            List<List<byte>> byteBlocks = new List<List<byte>>();
            List<byte> byteBlock_8 = new List<byte>();
            for (int i = 0; i < cipherBytes.Length; i++)
            {
                byteBlock_8.Add(cipherBytes[i]);

                if ((i + 1) % 8 == 0)
                {
                    byteBlocks.Add(byteBlock_8);
                    byteBlock_8 = new List<byte>();
                }
            }

            // Decrypt each 8-byte block of ciphertext.
            for (int i = 0; i < byteBlocks.Count; i++)
            {
                byteBlocks[i] = PassBytesIntoDES_Structure(byteBlocks[i].ToArray(), false).ToList<byte>();
            }

            // Combine all decrypted 8-byte blocks.
            List<byte> decryptedBytes = new List<byte>();
            foreach (List<byte> block in byteBlocks)
            {
                decryptedBytes.AddRange(block);
            }

            // Convert the last byte of the decrypted bytes into a decimal value.
            // If the value is between 1 (inclusive) and 8 (exclusive), then check if the number of 0x00 bytes before
            // the last byte equals the value - 1. If so, remove bytes from the end of the decrypted bytes equal to the
            // value; otherwise, do not remove any bytes.
            int paddingBytes;
            try
            {
                paddingBytes = Convert.ToInt32(decryptedBytes.Last());
            }
            catch (Exception ex)
            {
                return "The input is not a properly encrypted string.";
            }
            if (paddingBytes < 8)
            {
                bool allNullBytes = true;
                for (int i = 1; i < paddingBytes; i++)
                {
                    if (decryptedBytes[decryptedBytes.Count - 1 - i] != 0x00)
                    {
                        allNullBytes = false;
                        break;
                    }
                }

                if (allNullBytes)
                {
                    for (int i = 0; i < paddingBytes; i++)
                    {
                        decryptedBytes.RemoveAt(decryptedBytes.Count - 1);
                    }
                }
            }

            // Convert the decrypted bytes into plaintext with UTF-8 encoding.
            byte[] plainBytes = decryptedBytes.ToArray();
            string plainText;
            try
            {
                plainText = Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return plainText;
        }

        /// <summary>
        /// Passes 8 bytes (64 bits) through the DES structure and returns the result.
        /// If the process is for encryption, then 16 48-bit keys are applied to the rounds in the order they
        /// were generated in. Otherwise, the keys are applied in the reverse order.
        /// </summary>
        /// <param name="inputBytes"> the 8 bytes fed into the DES structure </param>
        /// <param name="forEncrypting"> Is this function being invoked for encryption? </param>
        /// <returns></returns>
        private static byte[] PassBytesIntoDES_Structure(byte[] inputBytes, bool forEncrypting)
        {
            // If there are not 8 input bytes, then return null.
            if (inputBytes == null || inputBytes.Length != 8)
            {
                return null;
            }

            // Convert the input bytes to bits.
            BitArray inputBits = new BitArray(inputBytes);

            // INITIAL PERMUTATION STAGE
            // Utilize the IP Table to alter the bit sequence in the input bits.
            BitArray inputCopy = new BitArray(inputBits);
            for (int i = 0; i < IP_TABLE.Length; i++)
            {
                inputBits[i] = inputCopy[IP_TABLE[i] - 1];
            }

            // Separate the input bits into 2 halves.
            BitArray leftBits_32 = new BitArray(32);
            BitArray rightBits_32 = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                leftBits_32[i] = inputBits[i];
                rightBits_32[i] = inputBits[i + 32];
            }

            // KEY GENERATION STAGE
            // Generate 16 48-bit keys based on the 64-bit key.
            BitArray[] keys = GenerateKeys(key);

            // Repeat the following 16 times.
            for (int round = 1; round <= 16; round++)
            {
                // Save the right 32 bits for the next round.
                BitArray rightBitsSave = new BitArray(rightBits_32);

                // EXPANSION PERMUTATION STAGE
                // Expand the 32 right bits into 48 bits by dividing the 32 bits into 8 4-bit blocks
                // and expanding those blocks into 6-bits each.

                // Copy the 32 right bits into the 8 4-bit blocks.
                // Copy the 32 right bits into the middle of the 8 6-bit blocks.
                BitArray[] bitBlocks_4 = new BitArray[8];
                BitArray[] bitBlocks_6 = new BitArray[8];
                for (int i = 0; i < 8; i++)
                {
                    bitBlocks_4[i] = new BitArray(4);
                    bitBlocks_6[i] = new BitArray(6);

                    for (int j = 0; j < 4; j++)
                    {
                        bitBlocks_4[i][j] = rightBits_32[i * 4 + j];
                    }
                }

                // For each 6-bit block, assign its 1st bit to be the last bit of the previous
                // 4-bit block and its 6th bit to be the 1st bit of the next 4-bit block.
                bitBlocks_6[0][0] = bitBlocks_4[7][3];
                bitBlocks_6[7][5] = bitBlocks_4[0][0];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bitBlocks_6[i][j + 1] = bitBlocks_4[i][j];
                    }

                    if (i > 0)
                    {
                        bitBlocks_6[i][0] = bitBlocks_4[i - 1][3];
                    }
                    if (i < 7)
                    {
                        bitBlocks_6[i][5] = bitBlocks_4[i + 1][0];
                    }
                }

                // Combine the 6-bit blocks.
                BitArray rightBits_48 = new BitArray(48);
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        rightBits_48[i * 6 + j] = bitBlocks_6[i][j];
                    }
                }

                // Perform XOR between the right 48 bits and 48-bit key.
                if (forEncrypting == true)
                {
                    rightBits_48 = rightBits_48.Xor(keys[round - 1]);
                }
                else
                {
                    rightBits_48 = rightBits_48.Xor(keys[16 - round]);
                }

                // Separate the right 46 bits into 6 bit blocks.
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        bitBlocks_6[i][j] = rightBits_48[i * 6 + j];
                    }
                }

                // S-BOX SUBSTITUTION
                // For each 6-bit block of the right 48 bits, collect the 4 middle bits and the 2 outer bits.
                // The 4 middle bits represent a column index whilst the 2 outer bits represent a row index.
                // Utilize the indices to collect an integer from the S-Box that corresponds with the current 6-bit block.
                // Convert this integer into 4 bits and replace the corresponding 4 bits of the right 32 bits with
                // these new bits.
                BitArray columnBit = new BitArray(4);
                BitArray rowBit = new BitArray(2);
                int[] sColumnIndices = new int[8];
                int[] sRowIndices = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    columnBit[0] = bitBlocks_6[i][4];
                    columnBit[1] = bitBlocks_6[i][3];
                    columnBit[2] = bitBlocks_6[i][2];
                    columnBit[3] = bitBlocks_6[i][1];

                    rowBit[0] = bitBlocks_6[i][5];
                    rowBit[1] = bitBlocks_6[i][0];

                    columnBit.CopyTo(sColumnIndices, i);
                    rowBit.CopyTo(sRowIndices, i);

                    byte[] subByte = null;
                    switch (i)
                    {
                        case 0:
                            subByte = BitConverter.GetBytes(S1[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 1:
                            subByte = BitConverter.GetBytes(S2[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 2:
                            subByte = BitConverter.GetBytes(S3[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 3:
                            subByte = BitConverter.GetBytes(S4[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 4:
                            subByte = BitConverter.GetBytes(S5[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 5:
                            subByte = BitConverter.GetBytes(S6[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 6:
                            subByte = BitConverter.GetBytes(S7[sRowIndices[i], sColumnIndices[i]]);
                            break;
                        case 7:
                            subByte = BitConverter.GetBytes(S8[sRowIndices[i], sColumnIndices[i]]);
                            break;
                    }

                    BitArray subBits = new BitArray(subByte);
                    for (int j = 0; j < 4; j++)
                    {
                        rightBits_32[i * 4 + j] = subBits[subBits.Length - 4 + j];
                    }
                }

                // P-BOX PERMUTATION STAGE
                // Utilize the P Table to alter the bit sequence in the right 32 bits.
                BitArray rightCopy = new BitArray(rightBits_32);
                for (int i = 0; i < P_TABLE.Length; i++)
                {
                    rightBits_32[i] = rightCopy[P_TABLE[i] - 1];
                }

                // XOR AND SWAP STAGE
                // Assign the right 32 bits for the next stage to the XOR result of the left 32 bits and the modified right 32 bits.
                // Assign the left 32 bits for the next stage to the original right 32 bits at the beginning.
                rightBits_32 = leftBits_32.Xor(rightBits_32);
                leftBits_32 = new BitArray(rightBitsSave);
            }

            // Combine the left and right 32 bits.
            bool[] combinedBits = new bool[64];
            rightBits_32.CopyTo(combinedBits, 0);
            leftBits_32.CopyTo(combinedBits, 32);
            BitArray outputBits = new BitArray(combinedBits);

            // FINAL PERMUTATION STAGE
            // Utilize the FP Table to alter the bit sequence in the output bits.
            BitArray cipherCopy = new BitArray(outputBits);
            for (int i = 0; i < FP_TABLE.Length; i++)
            {
                outputBits[i] = cipherCopy[FP_TABLE[i] - 1];
            }

            // Convert the output from bits to bytes.
            byte[] outputBytes = new byte[8];
            outputBits.CopyTo(outputBytes, 0);

            return outputBytes;
        }

        /// <summary>
        /// Generates and returns 16 6-byte (48-bit) keys based on a 8-byte (64-bit) input key and the DES structure.
        /// </summary>
        /// <param name="inputKey"> a 64-bit key </param>
        /// <returns> 16 48-bit keys </returns>
        private static BitArray[] GenerateKeys(byte[] inputKey)
        {
            // Initialize the 16 keys.
            BitArray[] keys = new BitArray[16];

            // Convert the input key from bytes to bits.
            BitArray keyBits = new BitArray(inputKey);

            // Reduce the input key to 56 bits utilizing the PC-1 Table to figure out which bit from the input
            // key should stay.
            BitArray key_56 = new BitArray(56);
            for (int i = 0; i < key_56.Count; i++)
            {
                key_56[i] = keyBits[PC_1[i] - 1];
            }

            // Divide the 56-bit key into 2 halves.
            BitArray leftBits_28 = new BitArray(28);
            BitArray rightBits_28 = new BitArray(28);
            for (int i = 0; i < 28; i++)
            {
                leftBits_28[i] = key_56[i];
                rightBits_28[i] = key_56[i + 28];
            }

            // Repeat the following for 16 rounds.
            for (int round = 1; round <= 16; round++)
            {
                // Conduct circular left shift on each key half.
                // For the 1st, 2nd, 9th, or 16th rounds, shift by 1 position.
                // Otherwise, shift by 2 positions.
                int shiftAmount = 2;
                if (round == 1 || round == 2 || round == 9 || round == 16)
                {
                    shiftAmount = 1;
                }

                for (int i = 0; i < shiftAmount; i++)
                {
                    bool leftTempBit = leftBits_28[0];
                    bool rightTempBit = rightBits_28[0];
                    for (int j = 1; j < leftBits_28.Count; j++)
                    {
                        leftBits_28[j - 1] = leftBits_28[j];
                        rightBits_28[j - 1] = rightBits_28[j];
                    }
                    leftBits_28[leftBits_28.Count - 1] = leftTempBit;
                    rightBits_28[rightBits_28.Count - 1] = rightTempBit;
                }

                // Combine the left and right bits into a 56-bit key.
                for (int i = 0; i < key_56.Count; i++)
                {
                    if (i < 28)
                    {
                        key_56[i] = leftBits_28[i];
                    }
                    else
                    {
                        key_56[i] = rightBits_28[i - 28];
                    }
                }

                // Reduce the 56-bit key to a 48-bit key utilizing the PC-2 Table to figure out which bit
                // from the 56-bit key should stay.
                BitArray key48 = new BitArray(48);
                for (int i = 0; i < key48.Count; i++)
                {
                    key48[i] = key_56[PC_2[i] - 1];
                }

                // Save the 48-bit key.
                keys[round - 1] = key48;
            }

            return keys;
        }
    }
}
