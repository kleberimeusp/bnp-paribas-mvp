import { Component, OnInit } from '@angular/core';
import { MovimentoManualService } from './movimento-manual.service';
import { MovimentoManual } from './movimento.model';
import { Cosif } from '../cosifs/cosif.model';
import { Produto } from '../produtos/produto.model';
import { ProdutosService } from '../produtos/produtos.service';
import { CosifsService } from '../cosifs/cosifs.service';

@Component({
  selector: 'app-movimentos',
  templateUrl: './movimento-manual.component.html',
  styleUrls: ['./movimento-manual.component.scss']
})
export class MovimentosComponent implements OnInit {
  movimentos: MovimentoManual[] = [];
  cosifs: Cosif[] = [];
  produtos: Produto[] = [];
  movimento: MovimentoManual = this.novoMovimento();
  editando = false;

  constructor(
    private movimentosService: MovimentoManualService,
    private produtosService: ProdutosService,
    private cosifsService: CosifsService
  ) {}

  ngOnInit(): void {
    this.carregarMovimentos();
    this.carregarProdutos();
    this.carregarCosifs();
  }

  carregarMovimentos(): void {
    this.movimentosService.listarTodos().subscribe({
      next: data => {
        this.movimentos = data.map(m => ({
          ...m,
          mes: m.mes.toString(),
          ano: m.ano.toString(),
          codigoProduto: m.codigoProduto?.trim(),
          codigoCosif: m.codigoCosif?.trim(),
          descricao: m.descricao?.trim()
        }));
      },
      error: err => console.error('Erro ao carregar movimentos:', err)
    });
  }

  carregarProdutos(): void {
    this.produtosService.listar().subscribe({
      next: data => {
        this.produtos = data.map(p => ({
          ...p,
          codigoProduto: p.codigoProduto?.trim(),
          descricao: p.descricao?.trim(),
          status: p.status?.trim(),
        }));
      },
      error: err => console.error('Erro ao carregar produtos:', err)
    });
  }

  carregarCosifs(): void {
    this.cosifsService.listarCosifs().subscribe({
      next: data => {
        this.cosifs = data.map(c => ({
          ...c,
          codigoCosif: c.codigoCosif?.trim(),
          status: c.status?.trim(),
        }));
      },
      error: err => console.error('Erro ao carregar cosifs:', err)
    });
  }

  salvar(): void {
    if (!this.movimento.codigoProduto || !this.movimento.codigoCosif || !this.movimento.descricao || !this.movimento.valor) {
      alert('Todos os campos obrigatórios devem ser preenchidos.');
      return;
    }

    this.movimento.codigoProduto = this.movimento.codigoProduto.trim();
    this.movimento.codigoCosif = this.movimento.codigoCosif.trim();
    this.movimento.descricao = this.movimento.descricao.trim();
    this.movimento.codigoUsuario = 'admin';
    this.movimento.produtoCosif = `${this.movimento.codigoProduto}-${this.movimento.codigoCosif}`;
    this.movimento.dataMovimento = new Date().toISOString();

    const acao = this.editando
      ? this.movimentosService.atualizar(this.movimento)
      : this.movimentosService.incluir(this.movimento);

    acao.subscribe({
      next: () => {
        this.carregarMovimentos();
        this.limpar();
      },
      error: err => alert('Erro ao salvar movimento: ' + (err.error?.erro || err.message))
    });
  }

  editar(m: MovimentoManual): void {
    this.movimento = {
      ...m,
      mes: m.mes.toString(),
      ano: m.ano.toString()
    };
    this.editando = true;
  }

  excluirMovimento(mov: MovimentoManual): void {
    if (confirm(`Deseja remover o movimento "${mov.descricao}"?`)) {
      this.movimentosService.remover(mov).subscribe({
        next: () => this.carregarMovimentos(),
        error: err => alert('Erro ao remover movimento: ' + (err.error?.erro || err.message))
      });
    }
  }

  limpar(): void {
    this.movimento = this.novoMovimento();
    this.editando = false;
  }

  novo(): void {
    this.movimento = this.novoMovimento();
    this.editando = false;
  }

  private novoMovimento(): MovimentoManual {
    const hoje = new Date();
    return {
      mes: (hoje.getMonth() + 1).toString(),
      ano: hoje.getFullYear().toString(),
      numeroLancamento: 0,
      codigoProduto: '',
      codigoCosif: '',
      descricao: '',
      valor: 0,
      codigoUsuario: 'admin',
      produtoCosif: '',
      dataMovimento: hoje.toISOString()
    };
  }
}
