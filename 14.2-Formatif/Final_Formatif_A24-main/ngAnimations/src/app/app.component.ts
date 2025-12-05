import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shakeX, tada } from 'ng-animate';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  animations: [
    trigger('bounce', [
      transition(':increment', useAnimation(bounce, {
        params: { timing: 0.5 }
      }))
    ]),
    trigger('shake', [
      transition(':increment', useAnimation(shakeX, {
        params: { timing: 1 }
      }))
    ]),
    trigger('tada', [
      transition(':increment', useAnimation(tada, {
        params: { timing: 2 }
      }))
    ])
  ]
})
export class AppComponent {
  title = 'ngAnimations';
  constructor() {
  }

  isAnimating: boolean = false;
  isRotating: boolean = false;

  bouceAnim: number = 0;
  shakeAnim: number = 0;
  tadaAnim: number = 0;

  async waitFor(delayInSeconds: number) {
    return new Promise(resolve => setTimeout(resolve, delayInSeconds * 1000));
  }

  async animated(boucle: boolean) {
    this.isAnimating = true;

    do {
      // 1️⃣ Shake rouge (2s)
      this.shakeAnim++;
      await new Promise(resolve => setTimeout(resolve, 2000));

      // 2️⃣ Bounce vert (4s)
      this.bouceAnim++;

      // 3️⃣ Tada bleu (3s) → démarre 1s avant la fin du bounce
      setTimeout(() => {
        this.tadaAnim++;
      }, 3000); // 4s bounce - 1s = 3s

      // Attendre la fin du bounce avant de recommencer
      await new Promise(resolve => setTimeout(resolve, 4000));

    } while (boucle);

    this.isAnimating = false;

  }
}

