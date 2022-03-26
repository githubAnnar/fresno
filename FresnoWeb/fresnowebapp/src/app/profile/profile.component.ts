import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDismissReasons, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TokenStorageService } from '../core/token-storage.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, AfterViewInit {
  @ViewChild('loggedOutModal') loggedOutModal: ElementRef | undefined;
  currentUser: any;
  redirectURL!: string;
  modalRef!: NgbModalRef;
  closeResult = '';

  constructor(private token: TokenStorageService, private modalService: NgbModal, private route: ActivatedRoute, private router: Router) {
    let params = this.route.snapshot.queryParams;
    if (params['redirectURL']) {
      this.redirectURL = params['redirectURL'];
    }
  }

  ngAfterViewInit(): void {
    console.log('ngAfterViewInit this.currentUser', this.currentUser);
    if (this.currentUser === undefined) {
      this.open(this.loggedOutModal);
    }
  }

  ngOnInit(): void {
    if (this.token.getToken()) {
      this.currentUser = this.token.getUser();
      console.log('if this.currentUser', this.currentUser);
    }
    else {
      console.log('else this.currentUser', this.currentUser);
    }
  }

  open(content: any) {
    const config: NgbModalOptions = {
      ariaLabelledBy: 'loginModalLabel',
      animation: true,
      centered: true,
      backdropClass: 'background-container'
    };

    this.modalRef = this.modalService.open(content, config);
    this.modalRef.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
      console.log(this.closeResult);
      this.redirect();
    },
      (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        console.log(this.closeResult);
        this.redirect();
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

  closeModal(): void {
    this.modalRef.close('Close button');
  }

  redirect(): void {
    if (this.redirectURL) {
      this.router.navigateByUrl(this.redirectURL)
        .catch(() => this.router.navigate(['/']))
    } else {
      this.router.navigate(['/'])
    }
  }
}
