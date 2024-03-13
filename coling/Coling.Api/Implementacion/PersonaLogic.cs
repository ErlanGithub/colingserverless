using Coling.Api.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Coling.Api.Afiliados.Implementacion
{
    public class PersonaLogic : IPersonaLogic
    {
        private readonly Contexto contexto;
        public PersonaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<bool> EliminarPersona(int id)
        {
            bool sw = false;
            Persona personaEliminar = await contexto.Personas.FirstOrDefaultAsync(x => x.Id == id);
            if (personaEliminar != null)
             {
                contexto.Remove(personaEliminar);
                 await contexto.SaveChangesAsync();
                return sw = true;
             }
            else { return sw; }
        }

        public async Task<bool> InsertarPersona(Persona persona)
        {
            bool sw = false;
            contexto.Personas.Add(persona);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Persona>> ListarPersonaTodos()
        {
            var lista = await contexto.Personas.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarPersona(Persona persona, int id)
        {
            bool sw = false;
            Persona personaModificar = await contexto.Personas.FirstOrDefaultAsync(x => x.Id == id);
            if (personaModificar != null)
            {
                personaModificar.Nombre = persona.Nombre;
                personaModificar.Apellidos = persona.Apellidos;
                personaModificar.FechaNacimiento = persona.FechaNacimiento;
                personaModificar.Foto = persona.Foto;
                personaModificar.Estado = persona.Estado;
                await contexto.SaveChangesAsync();          
                return sw = true;    
            }
            else { return sw; }
        }

        public async Task<Persona> ObtnerPersonaById(int id)
        {
            return await contexto.Personas.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
