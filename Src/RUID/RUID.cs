using System;
using System.Threading;

/**
 * A Random Unique/Universal ID.
 * RUIDs are uniformly distributed, are 24 ascii characters when encoded,
 * and are unique by 18 bytes of entropy.
 * 
 * Created by nfischer on 9/17/2016.
 * Ported to C# by csubj on 3/16/2018.
 */
namespace Csubj
{

    [Serializable]
    public class RUID : IComparable<RUID>
    {
        public static readonly int BINARY_SIZE = 18;
        private static readonly ThreadLocal<Random> RANDOM = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));


        private byte[] bytes { get; set; }
        private string encoded { get; set; }

        /**
	     * Create a RUID from source bytes
	     * @param bytes 18 random bytes
	     */
        public RUID(byte[] bytes)
        {
            setBytes(bytes);
        }

        public RUID(string encoded)
        {
            if(String.IsNullOrEmpty(encoded))
                throw new ArgumentNullException("Compact RUID cannot be null");

            byte[] bytes = Convert.FromBase64String(encoded);

            this.encoded = encoded;
            setBytes(bytes);
        }

        private RUID() { }

        /**
	     * Parse an RUID from a string.
	     * @param compact
	     * @throws IllegalArgumentException
	     */
        public static RUID Parse(String compact)
        {
            return new RUID(compact);
        }



        public static RUID Generate()
        {
            byte[] bs = new byte[BINARY_SIZE];
            RANDOM.Value.NextBytes(bs);
            RUID ruid = new RUID(bs);
            return ruid;
        }


        private void setBytes(byte[] bytes)
        {
            if(bytes != null)
                throw new ArgumentNullException("Compact RUID cannot be null");

            if(bytes.Length != BINARY_SIZE)
                throw new ArgumentException(String.Format("RUID must be %d bytes.", BINARY_SIZE));

            Array.Copy(bytes, this.bytes, BINARY_SIZE);
        }

        /**
         *
         * @return the binary representation of the RUID
         */
        public byte[] Bytes()
        {
            byte[] tmp = new byte[BINARY_SIZE];
            Array.Copy(bytes, tmp, BINARY_SIZE);
            return tmp;
        }

        /**
         * Returns the internal bytes of the RUID. Use this only when you need to
         * avoid memory allocation and know that the array will not be modified
         * @return the backing byte array of the RUID.
         */
        public byte[] RawBytes()
        {
            return bytes;
        }

        /**
         *
         * @return this RUID encoded as a encoded string
         */
        public string Encoded()
        {
            if(encoded != null)
                return encoded;

            return Convert.ToBase64String(this.bytes); ;
        }


        public override bool Equals(Object o)
        {
            if(this == o) return true;
            if(!(o is RUID)) return false;

            RUID ruid = (RUID) o;
            return Array.Equals(bytes, ruid.bytes);
        }


        public override int GetHashCode()
        {
            return bytes.GetHashCode();
        }

        public override String ToString()
        {
            return Encoded();
        }

        public int CompareTo(RUID other)
        {

            if(!(
                        this == other
                        || this.bytes == other.bytes
                        || (this.encoded != null
                            && this.encoded == other.encoded)
                ))
            {
                for(int i = 0;i < BINARY_SIZE;i++)
                {
                    int res = this.bytes[i] - other.bytes[i];
                    if(res != 0)
                        return res;
                }
            }
            return 0;
        }
    }
}
