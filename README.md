# 📘 Trabajo Final – Ingeniería de Software II  
## Gestión de Configuración del Software mediante GitFlow

---

## 📑 Tabla de Contenidos

- [Introducción](#-introducción)
- [Objetivos](#-objetivos)
  - [Objetivo General](#-objetivo-general)
  - [Objetivos Específicos](#-objetivos-específicos)
- [Marco Teórico](#-marco-teórico)
- [Estrategia de Ramificación – GitFlow](#-estrategia-de-ramificación--gitflow)
- [Diagrama de Flujo – GitFlow](#-diagrama-de-flujo--gitflow)
- [Flujo de Trabajo Aplicado](#-flujo-de-trabajo-aplicado)
- [Gestión de Versiones](#gestion-versiones)
- [Trazabilidad Formal](#-trazabilidad-formal)
- [Gestión del Cambio](#-gestión-del-cambio)
- [Resultados Obtenidos](#-resultados-obtenidos)
- [Desafíos y Consideraciones](#desafios-consideraciones)
- [Proyecto de Caso de Estudio](#-proyecto-de-caso-de-estudio)
- [Referencias](#-referencias)
- [Autores](#autores)

---

## 📌 Introducción

Este trabajo se enmarca en el área de **Gestión de Configuración del Software** (*Software Configuration Management – SCM*), disciplina fundamental de la Ingeniería de Software cuyo objetivo es controlar la evolución de los sistemas, gestionar cambios y garantizar la consistencia entre versiones.

El propósito del proyecto es demostrar, de manera práctica y reproducible, la aplicación de una estrategia de ramificación basada en **GitFlow**, utilizando **Git** y **GitHub** como herramientas de soporte.

Para ello, se desarrolló una aplicación web tipo Kanban que funciona como caso de estudio para evidenciar el proceso de versionado, gestión del cambio y liberación de versiones.

---

## 🎯 Objetivos

### 🎯 Objetivo General

Aplicar de manera práctica los conceptos de Gestión de Configuración del Software mediante la implementación de una estrategia de ramificación profesional basada en GitFlow.

### 🎯 Objetivos Específicos

- Implementar la estrategia GitFlow completa.
- Gestionar cambios mediante Issues.
- Garantizar trazabilidad entre:  
  **Issue → Rama → Commit → Pull Request → Merge → Tag**
- Simular liberación de versión estable (*Release*).
- Simular mantenimiento correctivo en producción (*Hotfix*).
- Aplicar versionado semántico.
- Documentar el proceso de manera reproducible.

---

## 🧠 Marco Teórico

La **Gestión de Configuración del Software (SCM)** tiene como objetivos principales:

- Identificar los elementos de configuración.
- Controlar cambios.
- Registrar y auditar versiones.
- Garantizar integridad y trazabilidad.
- Permitir reproducibilidad del sistema.

Dentro de SCM, el control de versiones y las estrategias de ramificación son mecanismos clave para organizar el desarrollo colaborativo y mantener estabilidad en el producto.

---

## 🌳 Estrategia de Ramificación – GitFlow

Se implementó la estrategia **GitFlow**, que define distintos tipos de ramas con responsabilidades específicas.

### 🔹 `main`

- Contiene versiones estables listas para producción.
- Solo recibe cambios desde ramas `release/*` o `hotfix/*`.

### 🔹 `develop`

- Rama de integración continua.
- Recibe todas las funcionalidades completadas.

### 🔹 `feature/*`

- Ramas temporales creadas desde `develop` para implementar nuevas funcionalidades.
- Cada feature está asociada a una Issue.

**Ejemplo:**
`feature/gestion-usuarios`

### 🔹 `release/*`

- Rama creada desde `develop` cuando el sistema está listo para liberarse.

**Ejemplo:**
`release/1.0.0`

Permite:

- Ajustes finales.
- Corrección de errores menores.
- Preparación de documentación.
- Creación del tag de versión.

### 🔹 `hotfix/*`

- Rama creada desde `main` para corregir errores detectados en producción.

**Ejemplo:**
`hotfix/correccion-error`

## 📊 Diagrama de Flujo – GitFlow

```mermaid
gitGraph
   commit id: "Initial Commit"

   branch develop
   checkout develop
   commit id: "Setup project structure"

   branch feature/gestion-usuarios
   checkout feature/gestion-usuarios
   commit id: "feat: user model"
   commit id: "feat: user repository"
   checkout develop
   merge feature/gestion-usuarios tag: "Merge feature"

   branch release/1.0.0
   checkout release/1.0.0
   commit id: "chore: version bump"
   checkout main
   merge release/1.0.0 tag: "v1.0.0"
   checkout develop
   merge release/1.0.0

   checkout main
   branch hotfix/correccion-error
   checkout hotfix/correccion-error
   commit id: "fix: bug fix"
   checkout main
   merge hotfix/correccion-error tag: "v1.0.1"
   checkout develop
   merge hotfix/correccion-error
---

## 🔄 Flujo de Trabajo Aplicado

Para cada funcionalidad se siguió el siguiente proceso:

1. Creación de Issue en GitHub.
2. Creación de rama `feature/*` desde `develop`.
3. Desarrollo con commits atómicos (Conventional Commits).
4. Push al repositorio remoto.
5. Creación de Pull Request hacia `develop`.
6. Code review obligatorio.
7. Merge.
8. Cierre automático de la Issue mediante `Closes #X`.

Este flujo garantiza trazabilidad completa y control formal del cambio.

---

## 🏷️ Gestión de Versiones <a id="gestion-versiones"></a>

Se aplicó **Versionado Semántico (SemVer)**:
```
MAJOR.MINOR.PATCH
```

### 🔹 v1.0.0

- Primera versión estable del sistema.
- Generada mediante rama `release/1.0.0`, merge a `main` y creación de tag anotado.

### 🔹 v1.0.1

- Corrección de error detectado tras la liberación inicial.
- Implementado mediante rama `hotfix/*`, merge a `main` y nuevo tag.

El uso de tags permite identificar de forma precisa el estado del sistema en cada versión liberada.

---

## 🔐 Trazabilidad Formal

Cada versión liberada puede reconstruirse exactamente mediante el uso de tags anotados, garantizando reproducibilidad del estado del sistema en cualquier punto del tiempo.

---

## 🔍 Gestión del Cambio

Cada cambio en el sistema fue gestionado mediante:

- **Issues** como unidad formal de solicitud de cambio.
- **Pull Requests** como mecanismo de revisión y aprobación.
- Eliminación de ramas tras su integración.
- Prohibición de trabajo directo en `main` o `develop`.

Esto permitió mantener control sobre la evolución del proyecto y evitar integraciones desordenadas.

---

## 📊 Resultados Obtenidos

La aplicación práctica de GitFlow permitió:

- Organización clara del desarrollo.
- Integración controlada de funcionalidades.
- Simulación realista de liberación de versiones.
- Simulación de mantenimiento correctivo.
- Historial de commits estructurado y auditable.
- Trazabilidad completa entre requerimientos y versiones liberadas.

---

## ⚠️ Desafíos y Consideraciones <a id="desafios-consideraciones"></a>

- Coordinación entre miembros del equipo.
- Disciplina en el uso de ramas.
- Gestión correcta de merges.
- Necesidad de documentación clara del flujo.

---

## 🚀 Proyecto de Caso de Estudio

Como soporte práctico se desarrolló una aplicación web tipo Kanban que permite:

- Gestión de usuarios.
- Creación y administración de tableros.
- Gestión de tareas.
- Asignación de responsabilidades.

El sistema fue utilizado únicamente como medio para demostrar la aplicación de la estrategia de configuración.

---

## 📚 Referencias

- Chacon, S., & Straub, B. *Pro Git*.
- Atlassian. *GitFlow Workflow*.
- IEEE Standard for Software Configuration Management.
- Documentación oficial de Git.

---

## 👨‍💻 Autores <a id="autores"></a>

**Campanini Goycochea Pablo Azul**  
**Lobo Rodrigo Alejandro**

Facultad de Ciencias Exactas y Tecnología  
Universidad Nacional de Tucumán