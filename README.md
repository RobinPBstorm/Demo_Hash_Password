# Problématiques de la Demonstration (sur la branche main)

1. L'enregistrement d'un nouvel utilisateur place le **mot de passe en clair**.

2. Les différents endpoints des controllers sont disponibles (même sans être connecté).

## 1. Hashage de Mot de Passe

### Recherche de librairie / Algorithme de hashage de mot de passe

- BCrypt
- Argon2
- SHA-516 (ou SHA-256)
  - Utilise HMAC
- SHA2-516 (ou SHA2-256)
  - Utilise HMAC
- PBKDF2
  - Utilise HMAC

#### A éviter

- Md5
- Base64

### Dans le cadre de ce projet

On devra faire un choix pour la librairie qui implémente un algorithme de hashage.

Ici, on en a ajouter 2 pour donner des exemples d'exploitation.

#### Dans la Business Logic Layer

Une interface **IHashService** décrit les services de Hashage qui devront gérer **HashPassword** et **Verify**.

Vous trouverez aussi 2 services qui implémente cette interface:
- **BCryptService:** Exploite BCrypt
- **Argon2Service:** Exploite Argon2

Dans tous les 2 exploitent **IConfiguration** pour prendre la configuration indiquée dans l'appsettings

Pour finir, on retrouvera l'injection de ce service dans l'**AuthService**.

#### Dans la couche API

Dans le **program.cs**, on peut choisir quel service.

```cs
builder.Services.AddScoped<IHashService, BCryptService>();
```

## 2. Le Json Web Token

### Mise en place simple

#### Ajout d'un middleware pour valider les tokens

Si le token reçut ne correspond pas selon les critères activés.
On recevra une erreur **401**

Dans le program.cs: 
```c#
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateLifetime = true,
           // ...
        };
    }
);
```

#### Création du Service de gestion du JWT

[Fichier du Service qui crée nos JWT](https://github.com/RobinPBstorm/Demo_Hash_Password/blob/TFArchiCybersecurite/DemoHashPasword.BLL/Services/JWTService.cs)

#### Ajout des règles 

Dans le program.cs: 
```C#
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("authPolicy", policy => policy.RequireAuthenticatedUser());
});
```

#### Indication des régles à appliquer sur les endpoints

Á indiquer devant le endpoint ou devant la classe du controller complète

```C#
[Authorize("authPolicy")]
[HttpGet("{id}")]
public IActionResult Get([FromRoute] int id)
{
  // ...
}
```

Pour permettre à des routes des demandes anonynes
```c#
[AllowAnonymous]
```

### Point de sécurité en plus (Recherche de groupe)

**Merci aux différents contributeurs pour cette section**

| **Attaque** |	**Protection** |
|--- | --- |
| alg=none | 	Forcer RequireSignedTokens |
| HS256 → RS256 | 	Forcer un algorithme unique |
| Rejeu (Replay Attack) | 	jti, tokens courts, refresh tokens |
| Vol de JWT via XSS | 	Cookies HttpOnly + CSP strict | 
| Brute force sur la signature | 	Clé longue et sécurisée |
| | Limiter les tentatives de déchiffrement par l'implémentation d'un mécanisme de vérouillage |
| Vol de clé secrète | Rotation régulière des clés |
| Injection de clé (Key Confusion) | 	Un seul algorithme, validation stricte |
| Détournement de sesion | Utilisation de HTTPS pour le transport de JWT |
| | JWT a vie courte et utilisation de refresh tokens |
| Usurpation d’issuer (iss falsifié) | 	ValidIssuer strict |
| Modification du payload |	Toujours valider le JWT côté serveur |
| Vol de Refresh Token |	Stockage en base, revocation, IP/device check |
 

#### Autres ressources

[Alternative au JWT](https://www.scottbrady.io/jose/alternatives-to-jwts)
