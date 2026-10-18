# Migrasyon 2D

Jeu de plateforme 2D inspiré de l'immigration au Québec dans les années 1970. Le joueur incarne tour à tour les membres d'une famille immigrante confrontés à l'hostilité de leur nouvel environnement.

![Statut](https://img.shields.io/badge/statut-en%20développement-orange)
![Genre](https://img.shields.io/badge/genre-2D%20Platformer-blue)

## Table des matières

- [Aperçu](#aperçu)
- [Fonctionnalités](#fonctionnalités)
- [Niveaux](#niveaux)
- [Installation](#installation)
- [Contrôles](#contrôles)
- [Technologies](#technologies)
- [Crédits](#crédits)
- [Équipe](#équipe)
- [Licence](#licence)

## Aperçu

Dans les années 1970, une famille arrive au Québec et se retrouve à la rue. Le père décroche rapidement un emploi en usine, mais y subit insultes et provocations en raison de ses origines. La mère, de son côté, affronte des situations hostiles dans son quotidien.

La progression est **linéaire** : le joueur suit l'histoire sans possibilité de s'en écarter.

| | |
|---|---|
| **Genre** | 2D Platformer |
| **Début du projet** | 9-22-2026 |
| **Dépôt** | [amineelghazi/migrasyon-2D](https://github.com/amineelghazi/migrasyon-2D) |

## Fonctionnalités

- Deux personnages jouables, un par niveau (Père, Mère)
- Mini-jeu de **Quick Time Event** pour la fabrication de ressorts
- **Barre de patience** déclenchant des combats lorsqu'elle atteint 100 %
- Ambiance visuelle en clair-obscur dans l'usine
- Progression narrative linéaire

## Niveaux

### Niveau 1 : Le Père

**Lieu :** Usine à BoxSpring

| Mécanique | Description |
|---|---|
| Déplacement et interaction | Le père se déplace et interagit avec la table de travail. |
| Quick Time Event | Le joueur clique sur la jauge pour fabriquer des ressorts. |
| Barre de patience | Elle augmente à chaque échec et sous l'effet des insultes des ouvriers. À 100 %, un combat contre le personnel de l'usine se déclenche. |

### Niveau 2 : La Mère

**Lieux :** En développement

> 🚧 Niveau en cours de développement.

## Installation

```bash
git clone https://github.com/amineelghazi/migrasyon-2D.git
cd migrasyon-2D
```

> À compléter : étapes pour ouvrir ou lancer le projet (moteur, version, build).

## Contrôles

| Action | Touche |
|---|---|
| Déplacement | ← ↑ ↓ → /  WASD |
| Saut | espace |
| Interagir | E |
| Quick Time Event | Clic de souris |

## Technologies

- Moteur / langage : Unity / C#
- Éditeur de niveaux : Unity Tilemap

## Crédits

- **Environnement :** [City Street Tileset Pack](https://muchopixels.itch.io/city-street-tileset-pack) par MuchoPixels
- **Sprites 2D (objets et personnages) :** Jason Laurin

## Équipe

- El Ghazi Amine
- Laurin Jason

## Licence

Copyright © 2026 El Ghazi Amine, Laurin Jason. Tous droits réservés.

Ce projet est protégé par la Loi sur le droit d'auteur (L.R.C. 1985, ch. C-42). Il est interdit de copier, modifier, distribuer, vendre ou réutiliser ce travail (code, sprites, animations), en tout ou en partie, sans l'autorisation écrite des auteurs. Le code est visible à des fins de consultation et d'évaluation uniquement.

Les ressources de tiers (par exemple le City Street Tileset Pack) restent soumises à leurs propres licences.

Cette licence est régie par les lois en vigueur au Québec et les lois fédérales du Canada qui s'y appliquent.
