import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, AbstractControl, FormControl,ValidatorFn } from '@angular/forms';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
users: User[] = [];
user: User = this.newUser();
message: string[] = [];
error = '';
signupMessage: string = '';
 // Array to add country codes for phone
countryCodes: string[] = [
  '+1 (United States)',
  '+44 (United Kingdom)',
  '+45 (Denmark)',
  '+49 (Germany)',
];
selectedCountryCode: string = '+45 (Denmark)'; // Set the default value
getCountryCode(country: string): string {
  // Extract the numeric part of the country code (e.g., '+1' from '+1 (United States)')
  const numericCode = country.match(/\+(\d+)/)?.[0];
  // If a numeric code is found, return it
  return numericCode || '';
}

RegisterForm! : FormGroup; // Declare the Register form
id: number= 0;  email:string= '';  firstName:string= '';  lastName:string= '';  password:string='';
address:string=''; city:string=''; country:string= ''; postalcode:string='';telephone:string='';role:number=2;


  constructor(
    private userService: UserService, // Inject UserService
    private router:Router,
    private fb: FormBuilder,
  ) {}


  ngOnInit(): void {
    this.getUsers();

    // initialization of  RegisterForm
    this.RegisterForm = this.fb.group({
      email: [this.email, [Validators.required,Validators.email,this.customEmailValidator]],
      firstName: [this.firstName, Validators.required],
      lastName: [this.lastName, Validators.required],
      password: [this.password, [Validators.required, Validators.minLength(6)]],
      address: [this.address],
      city: [this.city],
      countryCode:[this.countryCodes],
      //city: [this.city,  [Validators.pattern("/^[A-Za-z\s]*$/")]],
      country: [this.country],
      postalcode: [this.postalcode, [Validators.required, Validators.pattern('^[0-9]{4}$')]],
      telephone: [this.telephone, [Validators.pattern("^[0-9]{8,10}$")]],
      role: [this.role],
    });
  }

  newUser(): User {
    return { id: 0, email: '',  firstName: '', lastName: '',  password:'', address:'', city:'', country: '', postalcode:'',telephone:'',role:2};
  }

  getUsers(): void {
    this.userService.getUsers()
      .subscribe((a: User[]) => this.users = a);
  }

  cancel(): void {
    this.message = [];
    this.user = this.newUser();
    this.router.navigate(['/']);
  }

  getError(control:any) : string {
    if(control.errors?.required && control.touched)
      return 'This field is required!';
    else if(control.errors?.emailError && control.touched)
      return 'Please enter valid email address!';
    else return '';
}

 // Custom email validator
  customEmailValidator(control:AbstractControl):{ [key: string]: any } | null { {
    const pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    const value = control.value;

    if(!pattern.test(value) && control.touched)
      return { emailError:true }
    else {return null;}
}
  }
  // Custom phone number validator
  validatePhoneNumber(control: AbstractControl): { [key: string]: any } | null {
    // Define the regular expression pattern using RegExp object
    const pattern = new RegExp("^\\+[1-9]{1}[0-9]{0,2}-[2-9]{1}[0-9]{2}-[2-9]{1}[0-9]{2}-[0-9]{4}$");
    const value = control.value;

    // Test the phone number against the pattern
    if (!pattern.test(value) && control.touched) {
      console.log("Phone number is valid");
      return{ phoneError: true };
    } else {
      console.log("Phone number is not valid");
      return null;
    }
  }



//Save (Register) user function
  save(): void {
    if(this.RegisterForm.valid){
      //getting full phone number
      const countryCode = this.getCountryCode(this.RegisterForm?.get('countryCode')?.value);
      const telephoneNumber = this.RegisterForm.get('telephone')?.value;
      // Check if countryCode and telephoneNumber are not null or undefined before adding them
      if (countryCode != null && telephoneNumber != null) {
      // Concatenate the country code and telephone number
      const fullPhoneNumber = countryCode + ' ' + telephoneNumber;
      //'fullPhoneNumber' contains the complete phone number with country code
      console.log(fullPhoneNumber);

      //map form values to User object
      this.user.email= this.RegisterForm.get('email')?.value;
      this.user.password = this.RegisterForm.get('password')?.value;
      this.user.firstName= this.RegisterForm.get('firstName')?.value;
      this.user.lastName= this.RegisterForm.get('lastName')?.value;
      this.user.address= this.RegisterForm.get('address')?.value;
      this.user.city = this.RegisterForm.get('city')?.value;
      this.user.postalcode= this.RegisterForm.get('postalcode')?.value;
      this.user.country= this.RegisterForm.get('country')?.value;
      this.user.telephone= fullPhoneNumber;
      console.log("user email:", this.user.email);
      console.log(this.RegisterForm.getRawValue());


      if (this.user.id == 0) {
        console.log("user info:", this.user);
        this.userService.registerUser(this.user)
          .subscribe({
            next: (a: any) => {
            this.users.push(a)
            this.user = this.newUser();
            alert('Thanks for Signing Up!');
            this.signupMessage = 'Thanks for Signing Up!';
            this.RegisterForm.reset(); // Reset the form
           },
           error: (err: any)=>{
            this.signupMessage = 'User already exists!';
           // this.RegisterForm.reset(); // Reset the form
          }
        });
      } else {
            this.userService.updateUser(this.user.id, this.user)
              .subscribe(() => {
                this.user = this.newUser();
                this.signupMessage = 'User updated successfully.';
                this.RegisterForm.reset(); // Reset the form
              });
           }

  }}


 }
}

