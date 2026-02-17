# Trabajo Final – Ingeniería de Software II

## 📌 Descripción

Este proyecto corresponde al Trabajo Final de Ingeniería de Software II.

Consiste en el desarrollo colaborativo de un sistema de gestión de tareas con enfoque Kanban, que permite administrar tableros, tareas y usuarios con distintos niveles de permisos.

Durante el desarrollo se aplicaron principios y buenas prácticas de ingeniería de software:

* Arquitectura en capas
* Separación de responsabilidades
* Control de versiones profesional
* Integración continua
* Code review obligatorio
* Commits estandarizados
* Desarrollo basado en Issues

---

## 🎯 Objetivos del Proyecto

* Aplicar el patrón MVC
* Implementar acceso a datos desacoplado
* Trabajar con flujo Git profesional (feature branches + PR)
* Implementar testing automatizado
* Integrar CI mediante GitHub Actions
* Documentar decisiones técnicas

---

## 🏗️ Tecnologías utilizadas

* ASP.NET Core MVC
* C#
* SQLite
* ADO.NET
* Entity Framework Core
* Bootstrap
* JavaScript
* CSS
* Git & GitHub

---

## 🧠 Arquitectura

El sistema sigue el patrón MVC (Model-View-Controller) y aplica el patrón Repository para el acceso a datos.

## Capas del sistema

* Models → Entidades y lógica de dominio
* Views → Interfaz de usuario
* Controllers → Gestión de solicitudes HTTP
* Repositories → Acceso a datos desacoplado

## Principios aplicados

* Responsabilidad única (SRP)
* Separación de responsabilidades
* Bajo acoplamiento
* Código limpio
* Commits atómicos

---

## 🚀 Funcionalidades principales

✅ Registro e inicio de sesión de usuarios
✅ Gestión de tableros Kanban
✅ Creación, edición y eliminación de tareas
✅ Asignación de tareas a usuarios
✅ Seguimiento de estado de tareas
✅ Roles con distintos niveles de permisos
✅ Validaciones en formularios
✅ Manejo de sesiones
✅ Testing automatizado
✅ Integración continua

---

## 🌳 Estrategia de ramas

Se utiliza un flujo basado en GitFlow:

* main → versión estable lista para entrega
* develop → integración continua del equipo
* feature/* → desarrollo de nuevas funcionalidades

---

## 🔒 Reglas del repositorio

* ❌ No se trabaja directamente en **main**
* ❌ No se trabaja directamente en **develop**
* ✅ Toda funcionalidad nace desde **develop**
* ✅ Todo cambio ingresa mediante Pull Request
* ✅ Ningún PR se mergea sin review obligatorio

---

## 🔄 Flujo de trabajo (Pull Request)

* Crear Issue
* Crear rama desde **develop**
* Realizar commits atómicos
* Push al repositorio remoto
* Crear Pull Request hacia **develop**
* Code Review obligatorio
* Merge
* Cierre automático de Issue con Closes #X

---

## 📝 Convención de commits

Se utiliza el estándar **Conventional Commits**:

* **feat:** nueva funcionalidad
* **fix:** corrección de errores
* **docs:** documentación
* **style:** cambios de formato
* **chore:** tareas generales

---

## ✅ Checklist de revisión de Pull Request

* [ ] La rama parte de develop
* [ ] Los commits siguen Conventional Commits
* [ ] El código compila correctamente
* [ ] No hay warnings innecesarios
* [ ] Se respeta la arquitectura MVC
* [ ] Se aplicó principio de responsabilidad única
* [ ] No hay código muerto
* [ ] Se agregaron tests si corresponde
* [ ] La Issue está correctamente vinculada

---

## 👨‍💻 Autores

* Campanini Goycochea Pablo Azul - PU
* Lobo Rodrigo Alejandro - Ing. Informática