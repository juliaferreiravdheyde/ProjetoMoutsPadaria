using PontoFidelidadeAPI.Controllers;
using PontoFidelidadeAPI.Models;
using PontoFidelidadeAPI.Repositories;


namespace PontoFidelidadeAPI.Services
{
    public class FidelidadeService
    {
        private readonly FidelidadeRepository _repo;
        public FidelidadeService(FidelidadeRepository repo)
        {
            _repo = repo;
        }

        public Cliente Save(Cliente cliente)
        {
            ValidaDados(cliente);
            
            return _repo.Save(cliente);
        }

        public List<Cliente> GetAll()
        {
            return (List<Cliente>)_repo.GetAll();
        }

        public void Delete(int id)
        {
            Get(id);
            _repo.Delete(id);
        }

        public void Update(Cliente cliente)
        {
            ValidaDados(cliente);
            Get(cliente.Id);
            _repo.Update(cliente);
        }

        public Cliente Get(int id)
        {
            var clienteExistente = _repo.Get(id);
            if (clienteExistente == null)
            {
                throw new Exception("Pessoa não existe");
            }
            return _repo.Get(id);
        }

        private void ValidaDados(Cliente pessoa)
        {
            if (pessoa.Nome.Equals(""))
            {
                throw new Exception("O nome deve ser informado");
            }

            if (pessoa.CPF.Equals(""))
            {
                throw new Exception("CPF deve ser informado");
            }
        }
    }
}
