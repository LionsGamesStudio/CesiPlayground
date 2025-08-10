      
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

2.  **Ouvrir le Projet**
    -   Ouvrez le projet avec Unity Hub en utilisant la version 6.1.

3.  **Lancer le Client**
    -   Ouvrez la scène `Client_Boot` (`Assets/Client/Scenes/Client_Boot.unity`).
    -   Appuyez sur le bouton **Play**. Le client tentera de se connecter au serveur de jeu local.

## Structure du Projet Unity

Le projet est organisé en 4 assemblies principaux pour garantir une séparation claire des responsabilités :

-   **`Shared`** : Contient le code qui doit être connu par le Client et le Serveur (interfaces, classes de données, messages réseau).
-   **`Core`** : Contient les systèmes fondamentaux utilisés par les deux (ex: `ServiceLocator`, `EventBus`).
-   **`Client`** : Contient tout le code spécifique au client (logique de l'UI, rendu, capture d'input, Vues de gameplay).
-   **`Server`** : Contient tout le code spécifique au serveur (logique de jeu autoritaire, stratégies, IA, gestion des sessions).

## Mini-Jeux Implémentés

- **Gun Club** : Un jeu de tir sur cibles avec un système de vagues.
- **MollSmash** : Un jeu de type "tape-taupe" où le joueur doit frapper des taupes avec un marteau.

    