import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, ValidationErrors, Validators, ReactiveFormsModule, FormGroup, ValidatorFn } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [MatToolbarModule, MatIconModule, MatCardModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, ReactiveFormsModule, MatInputModule, MatButtonModule]
})
export class AppComponent {
  title = 'reactive.form';


  formGroup: FormGroup;
  constructor(private formBuilder: FormBuilder) {
    this.formGroup = this.formBuilder.group(
      {
        name: ['', [Validators.required]],
        roadnumber: ['', [Validators.required, Validators.min(1000), Validators.max(9999)]],
        postalCode: ['', Validators.pattern('^[A-Z][0-9][A-Z][ ]?[0-9][A-Z][0-9]$')],
        comments: ['', Validators.minLength(10)],

      },
      { validators: this.nomDansCommentaire() }
    );
  }

  nomDansCommentaire(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      // On récupère les valeurs de nos champs textes
      const commentaire = control.get('comments');
      const nom = control.get('name');
      // On regarde si les champs sont remplis avant de faire la validation
      if (!commentaire?.value || !nom?.value) {
        return null;
      }
      // On fait notre validation
      const estValide = !commentaire.value.includes(nom.value);

      if (!estValide) {
        // On ajoute l'erreur pour l'afficher sous le champ courriel
        // On conserve les autres erreurs déjà présentes
        commentaire.setErrors({ ...commentaire.errors, nomDansCommentaire: true });
      }

      return estValide ? null : { nomDansCommentaire: true };
    };
  }
}


