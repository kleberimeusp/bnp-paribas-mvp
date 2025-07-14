import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ProdutosComponent } from './produtos/produtos.component';
import { CosifsComponent } from './cosifs/cosifs.component';
import { MovimentosComponent } from './movimentos/movimentos.component'; // ✅ Se estiver usando Movimentos

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProdutosComponent,
    CosifsComponent,
    MovimentosComponent // ✅ Se existir
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,        // ✅ Corrige ngModel
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
