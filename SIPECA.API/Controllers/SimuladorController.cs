using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using SIPECA.Aplicacion;
using SIPECA.Aplicacion.DTOs;
using SIPECA.Dominio.Distribuciones.Continuas;
using SIPECA.Dominio.Distribuciones.Discretas;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.API.Controllers;

[ApiController]
[Route("simulador")]
public class SimuladorController: ControllerBase
{
    [HttpPost("simular")]
    public IActionResult Simular([FromBody] ParametrosSimulacion parametros)
    {
        var resultado = SimulacionService.Simular(parametros);
        return Ok(resultado);
    }
}
