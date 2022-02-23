import { invalid } from '@angular/compiler/src/render3/view/util';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() cancelRegister = new EventEmitter<boolean>();
  registerForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8),
      ]),
      confirmPassword: new FormControl('', [
        Validators.required,
        this.matchValues('password'),
      ]),
    });

    this.registerForm.get('password')?.valueChanges.subscribe(() => {
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();
    });
  }
  register() {
    // this.accountService.register(this.model).subscribe(
    //   (data) => {
    //     console.log(data);
    //     this.cancel();
    //   },
    //   (error) => {
    //     this.toaster.error(error.error);
    //     console.log(error);
    //   }
    // );
    console.log(this.registerForm.value);
  }
  cancel() {
    this.cancelRegister.emit(false);
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const controlValue = control.value;
      const matchingControl = (control?.parent as FormGroup)?.controls[matchTo];
      const matchControl = matchingControl?.value;
      return matchControl === controlValue ? null : { mismatch: true };
    };
  }
}
