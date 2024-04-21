using Microsoft.AspNetCore.Mvc;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using System.Threading.Tasks;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public interface IAutentifikacijaRepository
    {
        Task<object> Authenticate(string username, string password);
    }
}
