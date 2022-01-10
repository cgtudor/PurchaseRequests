﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PurchaseRequestsTests.Helpers
{
    public class FaultyValidator : IObjectModelValidator
    {

        public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix, object model)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                model, context, results,
                validateAllProperties: true
            );

            if (!isValid)
                results.ForEach((r) =>
                {
                    // Add validation errors to the ModelState
                    actionContext.ModelState.AddModelError("", r.ErrorMessage);
                });
            actionContext.ModelState.AddModelError("", "Generic Error");
        }
    }
}
