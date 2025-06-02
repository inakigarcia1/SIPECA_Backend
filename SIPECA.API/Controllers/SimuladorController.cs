using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using SIPECA.Aplicacion;
using SIPECA.Aplicacion.DTOs.Request;
using SIPECA.Dominio.Distribuciones.Continuas;
using SIPECA.Dominio.Distribuciones.Discretas;

namespace SIPECA.API.Controllers;

[ApiController]
[Route("sipeca")]
public class SimuladorController: ControllerBase
{
    [HttpPost("simular")]
    public IActionResult Simular([FromBody] ParametrosSimulacion parametros)
    {
        var resultado = SimulacionService.Simular(parametros);
        return Ok(resultado);
    }
}
