// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("jQ4ADz+NDgUNjQ4OD5GZUd+D7LC2vXXZWnevW8Kvz7HCPd6/hLziUve53phCeSyEtMKfsNp2vM+MPOZud4TGX8Zaj7tOW8iWhdcghjaHatGk0iE2keCrEbKgT6XadhYb7dEYQz+NDi0/AgkGJYlHifgCDg4OCg8MSbo+JUavMnnurnPCNRu/HgGuTfR26mbw+V6j96XNU9gDwTRAbnZ+c/C2MZQI+Rrl/aeCN52WW1Rfmyk3N4tDiqRieqGgNK5H98aftdTEbc1M4WoIpsUQjT7Ba6hIuZSeCtbOLndVXhTibsIKZ0oyMcKIHzVMkvg53LdPTvmMPFBcE0ec1soDLja3SU+pZVdhZ4e+fMFAUTRYN4vkcNOz0w+LVxaK7AHlRg0MDg8O");
        private static int[] order = new int[] { 1,11,10,10,12,11,10,7,8,10,13,12,12,13,14 };
        private static int key = 15;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
