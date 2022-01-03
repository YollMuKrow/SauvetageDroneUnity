# Scénario 
## Recherche et sauvetage de victime en essaim multi environnement

### Partie recherche de victimes : drone aérien
Des drones aériens en essaim vont parcourir une carte. Une fois une victime localisée, le drone va propager
la position vers la tour de contrôle. La tour va ensuite envoyer l'information aux drones terrestres pour que ceux-ci
aillent chercher les victimes.

### Partie Sauvetage de victime :
Une fois la localisation des victimes reçues par les drones terrestres, 
ils vont se rendre à la position, récupérer la victime et enfin, repartir à la zone de prise en charge des victimes.

# Fonctionnalités :
### Drones aériens :
X Spawn dans la zone prévue\
X déplacement a une certaine hauteur du sol\
X déplacement de manière pseudo-aléatoire sur une carte (on évite de repasser par les mêmes zones pendant un certain moment)\
X Détection de victimes (raycast)\
X Propagation de raycast depuis chaque drone dans un rayon définit\
X Dès qu'une victime est détectée par un raycast, on envoie sa position au drone terrestre\
O Une fois détecté, le drone est indisponible et surveille la victime\
O Une fois la victime prise en charge, le drone repart dans l'essaim de surveillance

### Drones terrestres :
X Spawn dans la zone prévue \
X Une fois une victime découverte et sa position reçue, le drone se rend jusqu'à la victime\
X Une fois à portée de la victime, il la récupère et retourne à la zone de prise en charge\
X les drones terrestres sont indépendants (si aucun drone n'est disponible, les victimes sont en attente)

### Menu:
Pouvoir changer :\
    X la hauteur de vol des drones\
    X nombre de drones aériens qui vont Spawn\
    X nombre de drones terrestres qui vont Spawn\
    X vitesse des différents drones et véhicules\
    X nombre de victimes qui vont Spawn\
    X Désactivation de la visualisation des raycast du drone
    
# Liste des imports : 
- Simple drone 1.0
- Terrain Tools 4.0.3
- Terrain Sample Asset Pack 2.0.0
- Distant lands free characters 1.2
- Free SpeedTrees Package 1.2

# Description d'un concept d'Unity :
### RigidBody :
Le rigidBody est un élément essentiel pour simuler la physique d'un objet sur Unity.
L'ajout du Rigidbody permet aux éléments (joueur, balle, ennemi, drone, ...) d'utiliser le moteur physique d'Unity sans pour autant ajouter du code.
Par exemple, après l'ajout d'un rigidBody, un élément sera attiré vers le sol en fonction de sa masse et de la gravité de l'environnement.
Pour le faire flotter (Dans le cas d'un drone de 10kg et d'une gravité de 9.81), le drone devrait déployer une force de 9.81x10 pour garder la même altitude.
Pour effectuer des rafraichissements sur la physique de l'objet et avoir une physique plus réalite, il est préférable d'appliquer la fonction FixedUpdate 
(qui est appelée au début de chaque image) plutôt que la méthode par défaut Update.
Du fait que le rigidbody utilise le moteur physique, les objets auront des comportements différents (mais plus réalite) que si on devait gérer la physique via l'objet lui-même.
Le rigidBody permet également via la méthode OnCollisionEnter de détecter si une collision avec un autre rigidBody a eu lieu. Le point de collision pouvant 
nous donner accès à l'objet nous est fourni.

# Comment utiliser le jeu :
- Télécharger le repo git
- Lancer l'exécutable dans le dossier "Build"
- Une fois démarré, les entités créées et les drones qui commencent à bouger, on peut utiliser :
    - ZQSD pour se déplacer (devant, gauche, derrière, droite)
    - espace/controle gauche pour monter/descendre.
    - A/E pour tourner la caméra à gauche/droite.
Pour le menu, on peut bouger les Sliders avec la souris ce qui modifiera en temps réel les objets sur la MAP(nombre, vitesse, altitude).

# Bug non résolue :
- Déplacer physiquement les victimes au sol par les robots terrestres
- Implémenter la répulsion quand 2 drones vont se rentrer dedans.
- Eviter la collision avec les arbres
