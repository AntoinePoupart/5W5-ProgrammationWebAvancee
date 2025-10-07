import { Component } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { MatButtonModule } from '@angular/material/button';


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    imports: [MatButtonModule]
})
export class AppComponent {
  title = 'Pizza Hub';

  private hubConnection?: signalR.HubConnection;
  isConnected: boolean = false;

  selectedChoice: number = -1;
  nbUsers: number = 0;

  pizzaPrice: number = 0;
  money: number = 0;
  nbPizzas: number = 0;

  constructor(){
    this.connect();
  }

  connect() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5282/hubs/pizza')
      .build();

    // TODO: Mettre isConnected à true seulement une fois que la connection au Hub est faite
    
  this.isConnected = true;
      this.hubConnection.on('UpdateNbUsers', (data) => {
      this.nbUsers = data;
    });
    
  this.isConnected = true;
      this.hubConnection.on('selectedChoice', (prix, nbPizza, money) => {
      this.pizzaPrice = prix;
      this.nbPizzas = nbPizza;
      this.money = money;
    });


    this.isConnected = true;
      this.hubConnection.on('addMoney', (data) => {
      this.money = data;
    });

    this.isConnected = true;
      this.hubConnection.on('buyPizza', (data) => {
      this.nbPizzas = data;
    });


      // On se connecte au Hub
    this.hubConnection
      .start()
      .then(() => {
        this.isConnected = true;
      })
      .catch(err => console.log('Error while starting connection: ' + err))

  }

  selectChoice(selectedChoice:number) {
    this.selectedChoice = selectedChoice;
    this.hubConnection!.invoke('SelectChoice', this.selectedChoice);
  }

  unselectChoice() {
    this.selectedChoice = -1;
      this.hubConnection!.invoke('UnselectedChoice', this.selectedChoice);
  }

  addMoney() {
    //Nom fonction backend + paramètre de la fonction
     this.hubConnection!.invoke('AddMoney', this.selectedChoice);
  }

  buyPizza() {
     this.hubConnection!.invoke('BuyPizza', this.selectedChoice);
  }
}
