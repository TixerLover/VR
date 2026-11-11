**What the VR?!** es un sandbox interactivo de Realidad Virtual creado con Unity 6 y el Meta XR SDK. Este proyecto funciona como una demostración técnica enfocada en las mecánicas físicas dentro de la VR, permitiendo a los jugadores explorar un archipiélago flotante mientras ponen a prueba la gravedad, el peso y las colisiones a través de diversos minijuegos.

## Resumen del Proyecto
El jugador inicia su experiencia en un conjunto de islas flotantes con una estética más fantasiosa que incluye dragones, leviatanes y zorros en capa. El entorno fue diseñado como un campo de pruebas para mecánicas de agarre y lanzamiento. Aquí podrás interactuar con diferentes objetos, desde un palo de golf hasta pinos de boliche y pelotas de golf de resorte, cada uno con configuraciones únicas de Rigidbody.

## Especificaciones Técnicas
Motor Gráfico: Unity 6.

SDK Integrado: Meta XR Core SDK / Building Blocks.

Hardware Requerido: Visor de la familia Meta Quest junto con sus respectivos controladores VR. O en su caso, el XR Meta Simulator desde el editor.

Despliegue: Soporte para pruebas directas en el editor vía Quest Link / Air Link, o exportación como archivo .apk. O en cualquier caso en el XR Meta Simulator desde el editor.

## Guía de Instalación y Ejecución
Obtener el proyecto: Clona este repositorio o descarga los archivos directamente a tu equipo.

Importar a Unity Hub: Abre Unity Hub, selecciona el botón Add y elige la carpeta donde extrajiste el proyecto. Asegúrate de abrirlo utilizando la versión correspondiente de Unity 6.

Cargar la escena base: Dentro del editor, ve a la pestaña Project, navega a la ruta Assets/MyFirstVRr/Scenes/ y haz doble clic sobre la escena MyFirstVr.

Vincular el casco VR: Enciende tus Meta Quest, inicia la aplicación Meta Horizon Link, conecta el visor a la computadora (por cable o de forma inalámbrica con Air Link) y aprueba la conexión de datos.

Comenzar a jugar: Haz clic en el botón Play ubicado en la parte superior del editor de Unity. Al ponerte el visor, aparecerás directamente en la isla de inicio.

## Características y Mecánicas
Físicas Personalizadas: El escenario incluye objetos con masas y propiedades elásticas muy distintas (una bola de golf, y diferentes pelotas que simulan una pelota normal, una que rebota mucho y una simular a una de boliche). Gracias al uso de diferentes PhysicMaterial, cada objeto ofrece una respuesta realista al rebotar, caer o ser manipulado con las manos virtuales.

Pista de Boliche: Una plataforma que cuenta con pinos de boliche y una pelota que respawnea después de usarla, cada pino hace un sonido al caer y cuando caen todos se reinician.

Una isla donde puedes meter pelotas de golf al hoyo y el contador se actualiza con cada una.

Sistema de Seguridad Anti-Caídas: Para prevenir el mareo y evitar que los objetos se pierdan, existe un volumen de colisión invisible debajo del nivel. Si el jugador, el palo de golf o cualquier pelota caen al vacío, son teletransportados al instante de vuelta a su punto de origen. 

Entorno Vivo: A través de scripts y componentes Animator, los elementos del mapa cuentan con animaciones cíclicas que crean una ilusión de levitación constante y suave en el aire. También hay dos modelos volando por el mapa, al igual que dragones en islas que se mueven.