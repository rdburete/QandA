/* eslint-disable prettier/prettier */
export const server =
	process.env.REACT_APP_ENV === 'production'
		? 'https://rdburete-qanda-backend.azurewebsites.net'
		: process.env.REACT_APP_ENV === 'staging'
			? 'https://rdburete-qanda-backend-staging.azurewebsites.net'
			: 'https://localhost:44334';

export const webAPIUrl = `${server}/api`;

export const authSettings = {
	domain: 'rdburete.eu.auth0.com',
	client_id: 'EKYPxGJk4IQhgaOudH8BfK84yx4lsWjj',
	redirect_uri: window.location.origin + '/signin-callback',
	scope: 'openid profile QandAAPI email',
	audience: 'https://qanda'
};
