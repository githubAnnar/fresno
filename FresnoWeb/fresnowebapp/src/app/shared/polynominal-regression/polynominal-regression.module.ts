import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class PolynominalRegressionModule {
  private params: number[][] = [];

  public fit(data: number[][], degrees: number[]) {
    degrees.forEach(degree => {
      const system = this.buildSystem(data, degree);
      const coefficients = this.solve(...system);
      this.params[degree] = coefficients;
    });
  };

  // Private 
  private buildArray(size: number): number[] {
    return Array<number>(size).fill(0);
  };

  private deepClone(arr: number[]) {
    return arr.map(x => Object.assign({}, x));
  };

  private addOperator(s: string): string {
    return s[0] === '-' ? s : '+' + s;
  };

  private getExpression(params: number[]) {
    return params.reduce((acc, el, i) => acc + this.addOperator(el.toString().replace('e', 'E')) + '*x^' + `${i}`, '');
  };

  private buildBase(degree: number): Function[] {
    return this.buildArray(degree + 1).map((_, i) => (x: number) => x ** i);

    // const buildBase = degree => buildArray(degree+1).map((_,i) => x => x**i);
  }

  private buildGramMatrix(data: number[][], base: Function[]): number[][] {
    return this.buildArray(base.length)
      .map((_, i) => this.buildArray(base.length)
        .map((_, j) => data.reduce((acc, [x]) => acc + base[i](x) * base[j](x), 0)));
  }

  private buildIndependentTerm(data: number[][], base: Function[]):number[][] {
    return this.buildArray(base.length).map((_, i) => Object.assign([], data.reduce((acc, [x, y]) => acc + base[i](x) * y, 0)));
  }

  private buildSystem(data: number[][], degree: number): [number[][], number[][]] {
    const base = this.buildBase(degree);
    const gramMatrix = this.buildGramMatrix(data, base);
    const independentTerm = this.buildIndependentTerm(data, base);
    return [gramMatrix, independentTerm];
  }

  private buildAugmentedMatrix(leftMatrix: number[][], rightMatrix: number[][]) {
    return leftMatrix.map((row, i) => row.concat(rightMatrix[i]));
  }

  private triangularize(augumented: number[][]): number[][] {
    const n = augumented.length;
    for (let i = 0; i < n - 1; i++) {
      for (let j = i + 1; j < n; j++) {
        const c = augumented[j][i] / augumented[i][i];
        for (let k = i + 1; k < n + 1; k++) {
          augumented[j][k] = augumented[j][k] - c * augumented[i][k];
        }
      }
    }

    return augumented;
  }

  private backSubstitute(augumentedMatrix: number[][]): number[] {
    const x: number[] = [];
    const n = augumentedMatrix.length;
    for (let i = n - 1; i >= 0; i--) {
      const alreadySolvedTerms = x.reduce((acc, val, idx) => acc + val * augumentedMatrix[i][n - 1 - idx], 0);
      x.push((augumentedMatrix[i][n] - alreadySolvedTerms) / augumentedMatrix[i][i]);
    }

    return x.reverse();
  }

  private solve(leftMatrix: number[][], rightMatrix: number[][]): number[] {
    return this.backSubstitute(this.triangularize(this.buildAugmentedMatrix(leftMatrix, rightMatrix)));
  }
}
