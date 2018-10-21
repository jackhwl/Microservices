import { FormGroup } from "@angular/forms";

export class ValidationErrorHandler {
    
  static handleValidationErrors(form: FormGroup, validationResult: any): void {
    //console.log('jjvalidationResult=', validationResult);
    for (var property in validationResult) {
      if (validationResult.hasOwnProperty(property)) {
        if (form.controls[property]){
          // single field
          var validationErrorsForFormField = {};
          for (var validationError of validationResult[property]) {
            validationErrorsForFormField[validationError.validatorKey] = true;
            //validationErrorsForFormField['message'] = validationError.message;
          }
          form.controls[property].setErrors(validationErrorsForFormField);
          //console.log('va=', validationErrorsForFormField);
          //console.log('setError=', this.tourForm.controls[property]);
        } else {
          // cross field
          var validationErrorsForForm = {};
          for (var validationError of validationResult[property]) {
            validationErrorsForForm[validationError.validatorKey] = true;
          }
          form.setErrors(validationErrorsForForm);
        }
      }
    }
  }

}
