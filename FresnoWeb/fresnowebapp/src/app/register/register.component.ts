import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDismissReasons, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  //encapsulation: ViewEncapsulation.None
})
export class RegisterComponent implements OnInit, AfterViewInit {
  @ViewChild('registerModal') registerModal: ElementRef | undefined;
  registerForm!: FormGroup;
  closeResult = '';

  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';
  modalRef!: NgbModalRef;
  redirectURL!: string;

  constructor(private authService: AuthService, private modalService: NgbModal, private router: Router, private route: ActivatedRoute) {
    let params = this.route.snapshot.queryParams;
    if (params['redirectURL']) {
      this.redirectURL = params['redirectURL'];
    }
  }

  ngAfterViewInit(): void {
    this.open(this.registerModal);
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      username: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  open(content: any) {
    const config: NgbModalOptions = {
      ariaLabelledBy: 'registerModalLabel',
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
    return this.registerForm.get('username');
  }

  get emailField(): any {
    return this.registerForm.get('email');
  }

  get passwordField(): any {
    return this.registerForm.get('password');
  }

  onSubmit(): void {
    const { username, email, password } = this.registerForm.value;

    this.authService.register(username, email, password).subscribe({
      next: data => {
        console.log(data);
        this.isSuccessful = true;
        this.isSignUpFailed = false;
        this.modalRef.close('Register OK');
      },
      error: err => {
        this.errorMessage = err.error.message;
        this.isSignUpFailed = true;
      }
    });
  }

  goLogin(): void {
    this.modalRef.close('Goto Login');
    this.router.navigate(['/login']);
  }
}
