import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('cours2');
  list : String [] = []
   result: any;
   constructor(private http: HttpClient) {}

    async testPrivate() {
      const url = 'https://localhost:7179/api/Account/PrivateTest'; 
      const response = await lastValueFrom(this.http.get<any>(url));
      this.list = response

    }
    
  async testPublic() {
    const url = 'https://localhost:7179/api/Account/PublicTest';  
    const response = await lastValueFrom(this.http.get<any>(url));
    this.list = response;
  }
    
  }

