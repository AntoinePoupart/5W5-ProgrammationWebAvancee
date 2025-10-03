import { inject } from '@angular/core';
import { CanActivateFn, createUrlTreeFromSnapshot } from '@angular/router';
import { UserService } from '../user.service';

export const preferCatGuard: CanActivateFn = (route, state) => {
 const userService = inject(UserService);
 const currentUser = userService.currentUser

 if(currentUser?.prefercat == false){
  return createUrlTreeFromSnapshot(route, ["/dog"]);
 }
 return true;

};

