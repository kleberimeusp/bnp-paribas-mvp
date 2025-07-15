import { Component, OnInit } from '@angular/core';
import { CosifsService } from './cosifs.service';
import { Cosif } from './cosif.model';
import { Produto } from '../produtos/produto.model';
import { MovimentosService } from '../movimentos/movimentos.service';

@Component({
  selector: 'app-cosifs',
  templateUrl: './cosifs.component.html',
  styleUrls: ['./cosifs.component.scss']
})
export class CosifsComponent implements OnInit {
  cosifs: Cosif[] = [];
  produtos: Produto[] = [];
  cosif: Cosif = this.novoCosif();
  editando = false;

  constructor(private cosifsService: CosifsService, private movimentosService: MovimentosService) {}

  ngOnInit(): void {
    this.listar();
    this.carregarProdutos();
  }

  listar(): void {
    this.cosifsService.listarCosifs().subscribe({
      next: data => {
        this.cosifs = data.map(c => ({
          ...c,
          codigoProduto: c.codigoProduto?.trim(),
          codigoCosif: c.codigoCosif?.trim(),
          codigoClassificacao: c.codigoClassificacao?.trim(),
          status: c.status?.trim(),
          produto: c.produto
            ? {
                ...c.produto,
                codigoProduto: c.produto.codigoProduto?.trim(),
                descricao: c.produto.descricao?.trim(),
                status: c.produto.status?.trim(),
                produtosCosif: c.produto.produtosCosif
              }
            : undefined
        }));
      },
      error: err => console.error('Erro ao buscar cosifs', err)
    });
  }


  carregarProdutos(): void {
    this.cosifsService.listarProdutos().subscribe({
      next: data => {
        this.produtos = data.map(p => ({
          ...p,
          codigoProduto: p.codigoProduto?.trim(),
          descricao: p.descricao?.trim(),
          status: p.status?.trim()
        }));
      },
      error: err => console.error('Erro ao buscar produtos', err)
    });
  }


  onSubmit(): void {
    // Encontra o objeto do produto selecionado no combo
    const produtoSelecionado = this.produtos.find(p => p.codigoProduto === this.cosif.codigoProduto);

    if (produtoSelecionado) {
      // Atualiza o objeto produto completo dentro do cosif
      this.cosif.produto = {
        codigoProduto: produtoSelecionado.codigoProduto,
        descricao: produtoSelecionado.descricao,
        status: produtoSelecionado.status,
        produtosCosif: []  // normal para evitar erro de serialização circular
      };

      // Atualiza também o código do produto explicitamente, caso necessário
      this.cosif.codigoProduto = produtoSelecionado.codigoProduto;
    }

    if (this.editando) {
      this.cosifsService.atualizar(this.cosif).subscribe(() => {
        this.listar();
        this.novo();
      });
    } else {
      this.cosifsService.incluir(this.cosif).subscribe(() => {
        this.listar();
        this.novo();
      });
    }
  }



  
  editar(c: Cosif): void {
    this.cosif = {
      codigoProduto: c.codigoProduto?.trim() || '',
      codigoCosif: c.codigoCosif?.trim() || '',
      codigoClassificacao: c.codigoClassificacao?.trim() || '',
      status: c.status || '',
      produto: {
        codigoProduto: c.produto?.codigoProduto?.trim() || c.codigoProduto,
        descricao: c.produto?.descricao || '',
        status: c.produto?.status || '',
        produtosCosif: c.produto?.produtosCosif || []
      },
      movimentos: c.movimentos ? [...c.movimentos] : []
    };

    this.editando = true;
  }


  excluir(codigoProduto: string, codigoCosif: string): void {
    if (confirm('Confirma exclusão?')) {
      // Primeiro: remover movimentos vinculados
      this.movimentosService.removerProdutoCosif(codigoProduto.trim(), codigoCosif.trim()).subscribe({
        next: () => {
          // Depois: remover o ProdutoCosif
          this.cosifsService.remover(codigoProduto.trim(), codigoCosif.trim()).subscribe({
            next: () => this.listar(),
            error: err => alert('Erro ao remover ProdutoCosif: ' + err.error?.erro || err.message)
          });
        },
        error: err => alert('Erro ao remover movimentos manuais: ' + err.error?.erro || err.message)
      });
    }
  }


  limpar(): void {
    this.novo();
  }

  novo(): void {
    this.cosif = this.novoCosif();
    this.editando = false;
  }

  private novoCosif(): Cosif {
    return {
      codigoProduto: '',
      codigoCosif: '',
      codigoClassificacao: '',
      status: '',
      produto: {
        codigoProduto: '',
        descricao: '',
        status: '',
        produtosCosif: []
      },
      movimentos: []
    };
  }

}
