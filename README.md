# SIPECA Backend 🧮

Backend del proyecto **SIPECA** (Simulador de Peras y Carpocapsa) implementado como **API REST en C# con .NET 7**. Se encarga de simular el ciclo de vida de la plaga *Carpocapsa* en cultivos de peras, con lógicas de distribución probabilística, ciclo biológico, e impacto de acciones de control (insecticida / feromonas).


## 📦 Repositorios enlazados

- **Frontend (UI React / Vite):**  
  https://github.com/santinohamada/SIPECA_Frontend

- **Backend (este repositorio):**  
  https://github.com/inakigarcia1/SIPECA_Backend


## 🧭 Descripción General

Exponemos una API que simula generación por generación el estado de un cultivo atacado por *Carpocapsa*, con datos de:

- Hectáreas e infectadas.
- Plantas por hectárea y frutos infectados.
- Políticas aplicadas: insecticidas, feromonas.
- Parámetros de simulación: número de generaciones, distribuciones aleatorias.

Se devuelve un desglose por generación (estado físico, daño, costos, etc.) y un resumen final de la temporada.


## 🛠️ Arquitectura & Tecnología

- **.NET 7** – Framework principal
- **ASP.NET Core Web API** – Servidor HTTP RESTful
- **Entity/Dependency Injection**, DTOs, componentes de simulación encapsulados.
- **Swashbuckle (Swagger)** – Documentación interactiva de endpoints
- **Configuración por appsettings.json** – Parámetros personalizados
- **XUnit o NUnit** – Tests unitarios para validar lógica simulación


## 📂 Estructura del Proyecto

```text
SIPECA_Backend/
├── Controllers/
│   └── SimulationController.cs      # Exposición HTTP de la simulación
├── Models/
│   ├── SimulationRequest.cs        # Parámetros de entrada
│   └── SimulationResponse.cs       # Respuesta generada
├── Services/
│   └── SimulationService.cs        # Lógica principal de simulación
├── Utils/
│   └── RandomHelpers.cs            # Métodos de generación aleatoria
├── Program.cs                      # Configuración del host
├── appsettings.json                # Configuración general
├── SIPECA_Backend.csproj
├── Tests/
│   └── SimulationTests.cs          # Pruebas unitarias
└── README.md
````

---

## 🔌 Endpoints Principales

### `POST /api/simulation`

Ejecuta la simulación.

#### Request Body

```json
{
    "cantidadHectareas": number,
    "plantasPorHectarea": number,
    "hectareasInfectadas": number,
    "costoTratamientoFeromonasPorHectarea": number,
    "costoTratamientoQuimicoPorHectarea": number,
    "precioPera": number,
    "aplicarQuimicos": boolean,
    "aplicarFeromonas": boolean
}
```

#### Response (Formato resumido)

```json
{
    "diasTotales": number,
    "generacionesTotales": number,
    "hectareasInfectadasFinales": number,
    "perasSanasFinales": number,
    "perasInfectadasFinales": number,
    "costoTotalTratamientoQuimico": number,
    "costoTotalTratamientoFeromonas": number,
    "dineroFinalGanado": number,
    "dineroFinalPerdido": number,
    "resultadosPorGeneracion": [
        {
            "generacion": 1,
            "dias": number,
            "hectareasInfectadas": number,
            "hectareasSanas": number,
            "perasSanas": number,
            "perasInfectadas": number,
            "ganancia": number,
            "perdida": number,
            "costoTratamientoQuimico": number,
            "costoTratamientoFeromonas": number
        },
        {
            "generacion": 2,
            ...
        },
        
    ]
}
```


## 🚀 Configuración Local

### Prerrequisitos

* [.NET 7 SDK](https://dotnet.microsoft.com/download)

### Pasos

1. **Clonar el repositorio**

   ```bash
   git clone https://github.com/inakigarcia1/SIPECA_Backend.git
   cd SIPECA_Backend
   ```

2. **Restaurar dependencias**

   ```bash
   dotnet restore
   ```

3. **Compilar y ejecutar**

   ```bash
   dotnet run --project SIPECA_Backend.csproj
   ```

4. **Swagger UI**

   * URL: `http://localhost:5000/swagger` (o según configuración en Program.cs)

## 🎯 Lógica de Simulación

El `SimulationService` incluye, entre otras, las siguientes distribuciones de probabilidad:

* **Distribución sexual 50/50 (dist. binomial)**.

* **Huevos por hembra (dist. normal):**
  * 45 ±10 en 1ª generación
  * 120 ±20 en subsiguientes

* **Supervivencia/diapausa (dist. binomial):**

  * 95% adultos / 5% diapausa (1ª generación)
  * 80% adultos / 20% diapausa (resto)

* **Insecticida (dist. binomial):**

  * 98% mortalidad (1ª gen)
  * 63% (resto)

* **Feromonas (dist. binomial):** 90% inhibición de puesta
* **Duración de ciclos biológicos (dist. exponencial):**

  * Eclosión: 6–11 días
  * Larva: 15–25 días
  * Pupa: 7–9 días

Se simula generación por generación, calculando daños físicos, económicos y tiempo acumulado.


## ✅ Testing

Se incluyen pruebas unitarias en `Tests/SimulationTests.cs`:

* Validación de distribución de huevos y sexos.
* Cálculo de impacto de insecticida y feromonas.
* Integridad del reporte final.

Ejecutar:

```bash
dotnet test
```


## 📜 Licencia

Este proyecto está bajo **MIT License**. Ver `LICENSE` para más detalles.

## 🎓 Créditos Académicos

Proyecto académico desarrollado para la materia **Simulación** en la carrera de **Ingeniería en Sistemas de Información** – **UTN-FRT**.

## 🧪 Próximos Pasos

* Persistencia en base de datos para historiales.
* Endpoints para informes históricos.
* Documentación adicional y métricas operativas.

## 👨‍💻 Autores

| Colaborador                                | Perfil                                       |
|--------------------------------------------|----------------------------------------------|
| ![Santino Hamada](https://github.com/santinohamada.png) | [Santino Hamada](https://github.com/santinohamada) |
| ![Iñaki Garcia](https://github.com/inakigarcia1.png) | [inakigarcia1](https://github.com/inakigarcia1) |
| ![Matias Vel](https://github.com/MatiasVel.png) | [MatiasVel](https://github.com/MatiasVel) |
| ![Emmanuel Arnedo](https://github.com/emmanuelarnedo.png) | [emmanuelarnedo](https://github.com/emmanuelarnedo) |
