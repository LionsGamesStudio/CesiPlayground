# CESI Playground

CESI Playground est un projet de fête foraine en réalité virtuelle (VR) conçu avec une architecture client-serveur moderne, permettant des mini-jeux multijoueurs.

## Architecture d'Ensemble

Le projet est conçu pour fonctionner avec un serveur de jeu dédié et autoritaire.

1.  **Serveur de Jeu Dédié** : Un build "headless" du projet Unity qui gère toute la logique de jeu en temps réel (déplacements, physique, règles du jeu). Il est la source de vérité pour l'état du jeu.

2.  **Client de Jeu VR** : Le projet Unity principal qui s'exécute sur la machine du joueur. Il est responsable du rendu graphique, de la capture des inputs VR, et de l'affichage de l'état du jeu tel que dicté par le serveur.

La communication entre le client et le serveur se fait via des WebSockets.

## Technologies Utilisées

- **Moteur de Jeu** : Unity 6.1
- **Réseau (Temps Réel)** : WebSockets (via la librairie `WebSocket-Sharp`)
- **Langage** : C# 10+
- **Librairies Clés** :
  - **Newtonsoft.Json** : Pour la sérialisation/désérialisation des messages réseau.
  - **Unity XR Interaction Toolkit** : Pour les interactions en Réalité Virtuelle.

## Installation et Lancement

Suivez ces étapes pour lancer un environnement de test local.

### Prérequis
- **Unity Hub**
- **Unity 6.1** (avec les modules **Windows Build Support (IL2CPP)** et **Linux Build Support (IL2CPP)** installés)
- Un serveur de jeu dédié en cours d'exécution.

### Étapes de Lancement
1.  **Cloner le Dépôt**
    ```bash
    git clone [URL_DE_VOTRE_DEPOT]
    cd CesiPlayground
    ```

2.  **Configurer l'Environnement**
    -   Avant la première utilisation, configurez votre environnement de développement. Voir la section **"Gestion de l'Environnement"** plus bas.

3.  **Lancer le Client**
    -   Ouvrez le projet avec Unity Hub en utilisant la version 6.1.
    -   Ouvrez la scène `Client_Boot` (`Assets/Client/Scenes/Client_Boot.unity`).
    -   Appuyez sur le bouton **Play**. Le client tentera de se connecter au serveur de jeu local.

## Structure du Projet Unity

Le projet est organisé en 4 assemblies principaux pour garantir une séparation claire des responsabilités :

-   **`Shared`** : Contient le code qui doit être connu par le Client et le Serveur (interfaces, classes de données, messages réseau).
-   **`Core`** : Contient les systèmes fondamentaux utilisés par les deux (ex: `ServiceLocator`, `EventBus`).
-   **`Client`** : Contient tout le code spécifique au client (logique de l'UI, rendu, capture d'input, Vues de gameplay).
-   **`Server`** : Contient tout le code spécifique au serveur (logique de jeu autoritaire, stratégies, IA, gestion des sessions).
-   **`Editor`** : Contient les scripts d'outils utilisés uniquement dans l'Éditeur Unity.

## Outils d'Éditeur

Ce projet inclut des outils personnalisés pour automatiser le workflow de développement. Ils sont accessibles via des menus en haut de l'éditeur.

### Gestion de l'Environnement

Le jeu utilise des fichiers de configuration JSON pour gérer les URLs de l'API et du serveur de jeu.

1.  **Configuration Initiale** :
    -   Dans le dossier `Assets/Config/`, vous trouverez des fichiers modèles (`.example`).
    -   Pour la production, copiez `environment.prod.json.example` et renommez-le en `environment.prod.json`. Remplissez-le avec vos URLs de production. Ce fichier est ignoré par Git et restera privé.

2.  **Changer d'Environnement** :
    -   Utilisez le menu **`Environnement`** en haut de l'éditeur.
    -   **`Switch to Development`** : Configure le jeu pour utiliser les URLs locales (`localhost`). À utiliser pour les tests dans l'éditeur.
    -   **`Switch to Production`** : Configure le jeu pour utiliser les URLs de production. À utiliser avant de faire un build final.

### "Baking" des Données de Scène

Pour que le serveur connaisse la disposition des niveaux sans charger les scènes visuelles, nous "cuisons" (bake) les informations de position dans des `ScriptableObject`s.

-   **`Tools > Bake Hub Stand Layout`** : À lancer lorsque la scène `Hub` est ouverte. Cet outil scanne tous les stands de jeu (`GameStand`) et sauvegarde leur configuration pour le serveur.
-   **`Tools > Bake Scene Spawn Points`** : À lancer lorsque qu'une scène de mini-jeu est ouverte (ex: `GunClub_Scene`). Cet outil scanne tous les marqueurs de points de spawn (`SpawnPointMarker`) et sauvegarde leurs positions pour le serveur.

**Important** : Le "baking" doit être refait à chaque fois que vous modifiez la position ou l'agencement des stands dans le Hub ou des points de spawn dans un mini-jeu.

### Builds Automatisés

Le menu **`Build`** permet de créer les différentes versions du jeu de manière sûre et reproductible.

-   **`Build > Client`** : Contient les options pour compiler le client VR pour différentes plateformes (Windows, Android) et différents profils de casques (Oculus, OpenXR). Ce processus désactive automatiquement le simulateur XR pour le build et restaure vos paramètres d'éditeur ensuite.
-   **`Build > Server`** : Contient l'option pour compiler le serveur dédié headless pour Linux. Ce processus désactive automatiquement la XR pour le build.

## Mini-Jeux Implémentés

- **Gun Club** : Un jeu de tir sur cibles avec un système de vagues.
- **MollSmash** : Un jeu de type "tape-taupe" où le joueur doit frapper des taupes avec un marteau.