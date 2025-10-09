import { inject } from '@angular/core';
import { CanActivateFn, createUrlTreeFromSnapshot } from '@angular/router';
import { UserService } from '../user.service';


export const prefercatGuard: CanActivateFn = (route, state) => {
    // On inject le service pour regarder si l'utilisateur est connecté
    if (!inject(UserService).preferCat())
        // S'il n'est pas connecté on le redirige vers la page de login
        return createUrlTreeFromSnapshot(route, ["/dog"]);
    // S'il est connecté, tout est beau on continue!
    else return true;
};


//**Peut faire ca sans toucher au service en injectant userService et le current User**

// export const preferCatGuard: CanActivateFn = (route, state) => {
//     const userService = inject(UserService);
//     const currentUser = userService.currentUser

//     if (currentUser?.prefercat == false) {
//         return createUrlTreeFromSnapshot(route, ["/dog"]);
//     }
//     return true;

// };