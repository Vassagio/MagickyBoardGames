using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MagickyBoardGames.ViewModels;
using Moq;

namespace MagickyBoardGames.Tests.Mocks {
    public class MockValidator<T> : IValidator<T> where T : IViewModel {
        public CascadeMode CascadeMode { get; set; }
        private readonly Mock<IValidator<T>> _mock;

        public MockValidator() {
            _mock = new Mock<IValidator<T>>();
        }

        public ValidationResult Validate(object instance) {
            return _mock.Object.Validate(instance);
        }

        public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = new CancellationToken()) {
            return _mock.Object.ValidateAsync(instance, cancellation);
        }

        public ValidationResult Validate(ValidationContext context) {
            return _mock.Object.Validate(context);
        }

        public IValidatorDescriptor CreateDescriptor() {
            return _mock.Object.CreateDescriptor();
        }

        public bool CanValidateInstancesOfType(Type type) {
            return _mock.Object.CanValidateInstancesOfType(type);
        }

        public Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = new CancellationToken()) {
            return _mock.Object.ValidateAsync(context, cancellation);
        }

        public ValidationResult Validate(T instance) {
            return _mock.Object.Validate(instance);
        }

        public Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = new CancellationToken()) {
            return _mock.Object.ValidateAsync(instance, cancellation);
        }

        public MockValidator<T> ValidateStubbedToReturnValid() {
            var results = new ValidationResult();
            _mock.Setup(m => m.Validate(It.IsAny<T>())).Returns(results);
            return this;
        }

        public MockValidator<T> ValidateStubbedToReturnInvalid() {
            var results = new ValidationResult();
            results.Errors.Add(new ValidationFailure("Propery", "Error"));
            _mock.Setup(m => m.Validate(It.IsAny<T>())).Returns(results);
            return this;
        }

        public void VerifyValidateCalled(T viewModel, int times = 1) {
            _mock.Verify(m => m.Validate(viewModel), Times.Exactly(times));
        }
    }
}