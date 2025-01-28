# Problématique de Demonstration (sur la branche main)

L'enregistrement d'un nouvel utilisateur place le **mot de passe en clair**.

## Recherche de librairie / Algorithme de hashage de mot de passe

- BCrypt
- Argon2
- SHA-516 (ou SHA-256)
  - Utilise HMAC
- SHA2-516 (ou SHA2-256)
  - Utilise HMAC
- PBKDF2
  - Utilise HMAC

### A éviter

- Md5
- Base64

## Dans le cadre de ce projet

On devra faire un choix pour la librairie qui implémente un algorithme de hashage.

Ici, on en a ajouter 2 pour donner des exemples d'exploitation.

### Dans la Business Logic Layer

Une interface **IHashService** décrit les services de Hashage qui devront gérer **HashPassword** et **Verify**.

Vous trouverez aussi 2 services qui implémente cette interface:
- **BCryptService:** Exploite BCrypt
- **Argon2Service:** Exploite Argon2

Dans tous les 2 exploitent **IConfiguration** pour prendre la configuration indiquée dans l'appsettings

Pour finir, on retrouvera l'injection de ce service dans l'**AuthService**.

### Dans la couche API

Dans le **program.cs**, on peut choisir quel service.

```cs
builder.Services.AddScoped<IHashService, BCryptService>();
```
