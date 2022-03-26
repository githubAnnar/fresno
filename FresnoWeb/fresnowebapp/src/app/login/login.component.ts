import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDismissReasons, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../core/auth.service';
import { TokenStorageService } from '../core/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {
  @ViewChild('loginModal') loginModal: ElementRef | undefined;
  loginForm!: FormGroup;
  closeResult = '';

  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';
  roles: string[] = [];
  modalRef!: NgbModalRef;
  redirectURL!: string;

  constructor(private authService: AuthService, private tokenStorage: TokenStorageService, private modalService: NgbModal, private router: Router, private route: ActivatedRoute) {
    let params = this.route.snapshot.queryParams;
    if (params['redirectURL']) {
      this.redirectURL = params['redirectURL'];
    }
  }

  ngAfterViewInit(): void {
    this.open(this.loginModal);
  }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
      this.roles = this.tokenStorage.getUser().roles;
    }
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  open(content: any) {
    const config: NgbModalOptions = {
      ariaLabelledBy: 'loginModalLabel',
      backdrop: 'static',
      keyboard: false,
      animation: true,
      centered: true,
      backdropClass: 'background-container'
    };

    this.modalRef = this.modalService.open(content, config);
    this.modalRef.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      console.log(this.closeResult);
    },
      (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        console.log(this.closeResult);
      });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  get usernameField(): any {
    return this.loginForm.get('username');
  }
  get passwordField(): any {
    return this.loginForm.get('password');
  }

  get loginFailed(): any {
    return this.isLoginFailed;
  }

  onInputChange(): void {
    this.isLoginFailed = false;
  }

  onSubmit(): void {
    const { username, password } = this.loginForm.value;

    this.authService.login(username, password).subscribe({
      next: data => {
        this.tokenStorage.saveToken(data.accessToken);
        this.tokenStorage.saveUser(data);

        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.roles = this.tokenStorage.getUser().roles;
        this.modalRef.close('Login OK');
        this.redirect();
      },
      error: err => {
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
        console.log(this.isLoginFailed, this.errorMessage);
      }
    });
  }

  redirect(): void {
    if (this.redirectURL) {
      this.router.navigateByUrl(this.redirectURL)
        .catch(() => this.router.navigate(['/']))
    } else {
      this.router.navigate(['/'])
    }
  }

  goRegister(): void {
    this.modalRef.close('Goto Register');
    this.router.navigate(['/register']);
  }
}
