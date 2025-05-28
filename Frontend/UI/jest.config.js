module.exports = {
  preset: 'jest-preset-angular',
  globalSetup: 'jest-preset-angular/global-setup',
  moduleNameMapper: {
    '\\.(scss|sass|css)$': 'identity-obj-proxy'
  }
};
