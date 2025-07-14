import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProdutosComponent } from './produtos/produtos.component';
import { CosifsComponent } from './cosifs/cosifs.component';
import { MovimentosComponent } from './movimentos/movimentos.component'; // ✅ Importa Movimentos

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'produtos', component: ProdutosComponent },
  { path: 'cosifs', component: CosifsComponent },
  { path: 'movimentos', component: MovimentosComponent }, // ✅ Adiciona rota movimentos
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
