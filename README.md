# GemRush
### 09/04/2022 Primera subida
Búsqueda de información para usar Unity, creación, subida de repositorios y búsqueda de ideas para poder hacer el juego.
Proyecto básico con 4 pantallas y un cambio de escenas sencillo.
La clase Gema tendrá las propiedades y funciones necesarias para su implementeación y funcionamiento en el juego.

### 18/04/2022 Segunda subida
Modificada la clase Gema. 
Creada las clases TipoGema y Cuadrucula.
La clase Gema indicará la posición de cada una y su tipo. También será la que nos permita hacer los movimientos.
La clase Cuadricula nos pondrá un fondo a cuadros y colocará cada gema encima de un cuadrado.
La clase TipoGema nos indicará las catacterísticas de la gema (como el tipo, el sprite que tendrá, etc).

### 24/04/2022 Tercera subida
Añadida una propiedad para el movimiento de las gemas.
Añadida la función para rellenar los cuadrados con distintos tipos de gemas (contiene un error del tipo NullPointerException)

### 25/04/2022 Cuarta subida
Solucionado el error de la subida anterior.
Añadida una animación para rellenar la cuadrícula con distintas gemas.

### 02/05/2022 Quinta subida
Añadido el código necesario que permita intercambiar gemas de posición. Por el momento se intercambiarán si o si, no cuando hagan una línea.
También tengo hay fragmento de código que sirve para colocar los típicos "muros" que tiene el Candy Crush, solo que como da error, queda comentado.

### 02/05/2022 Sexta subida
Creadas funciones que permitan hacer las cambinaciones. Por ejemplo, una linea de 3, 2 líneas en forma de L y T.
Creada una pequeña animación con Unity que simulará la desaparición de una gema al hacer una de estas combinaciones.
Estes últimos cambios están sin implementar correctamente.

### 04/05/2022 Séptima subida
Revertidos algunos de los cambios anteriores para solucionar errores.
Se ha eliminado la animación y comentado el código que permita hacer combinaciones en forma de T y L temporalmente. La animación impedía que se rellenase la cuadrícula.
Solucionado el fallo de rellenar la cuadrícula, ahora se rellena de abajo hacia arriba y no al revés.
Se han solucionado algunos errores relacionados con los muros, igualmente siguen comentados por otros fallos.

### 06/05/2022 Octava subida
Se ha creado una animación que al hacer una combinación, esta se desvanece. La animación no se ha implementado porwue hace que aparezcan y desaparezcan constantemente todas las gemas (es molesto).
Se han añadido 3 nuevos tipos de gemas, pero por el momento solo se han introducido 2 tipos en la cuadrícula. Se han redimensionado los tamaños de las gemas Rojas, Verdes y Naranjas
Se ha descomentado el código que permitía eliminar las combinaciones, aunque algunas no deaparecen correctamente.

### 09/05/2022 Novena subida
Creadas nuevas animaciones que se activan cuando haces una combinacion de 4 gemas (no esta bien implementada).
Se ha creado una función que permita romper los muros (aunque la cuadrícula no se rellena correctamente por el momento).
Se han creado 2 funciones que permitan limpiar filas y columnas (es el "poder" de las combinaciones de 4 gemas).
Creada la clase de las gemas especiales (combinación de 4) que tiene la función que permite limpiar filas y columnas. 



