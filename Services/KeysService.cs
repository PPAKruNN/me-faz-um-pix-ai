using FazUmPix.DTOs;
using FazUmPix.Repositories;

namespace FazUmPix.Services;

public class KeysService(KeysRepository keysRepository)
{
    public async Task<ReadKeyOutputDTO> ReadKey(ReadKeyInputDTO dto)
    {
        ReadKeyOutputDTO key = await keysRepository.Read(dto);

        return key;
    }
}

