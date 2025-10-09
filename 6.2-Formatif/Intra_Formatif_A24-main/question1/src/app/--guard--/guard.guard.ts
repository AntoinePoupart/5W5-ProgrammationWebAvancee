import { CanActivateFn, createUrlTreeFromSnapshot } from '@angular/router';
import { UserService } from '../user.service';
import { inject } from '@angular/core';

export const guardGuard: CanActivateFn = (route, state) => {
  // On inject le service pour regarder si l'utilisateur est connecté
  if (!inject(UserService).isLogged())
    // S'il n'est pas connecté on le redirige vers la page de login
    return createUrlTreeFromSnapshot(route, ["/login"]);
  // S'il est connecté, tout est beau on continue!
  else return true;
};

//Peut faire une fonction ici aussi mais mieux dans le service
// function isLogged(){
//   //LocalStorage car dans (user.service) utilise un localstorage et non un sessionStorage
//   if(localStorage.getItem("user") != null){
    
//      return true;
//   }
//   console.log("sa fonctionne");
//   return false;
// }


