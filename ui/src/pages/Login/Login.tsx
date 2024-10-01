import React, { ChangeEvent, MouseEvent, useState } from 'react'

// Our stuff
import './Login.css'
import Title from '../../components/title/Title';
import { useHttpRequestService } from '../../contexts/HttpRequestServiceContext';
import UriBuilder from '../../utils/UriBuilder';
import { getLoginConfig, IUserLogin } from '../../configs/form-config';
import InputForm from '../../components/input-form/InputForm';
import { IHttpRequestService } from '../../services/interfaces/IHttpRequestService';

const Login = (): JSX.Element => {
  const [userLogin, setUserLogin] = useState<IUserLogin>({ email: '', password: '' });
  const httpRequestService: IHttpRequestService = useHttpRequestService();

  const handleLogin = async (event: MouseEvent<HTMLInputElement>): Promise<void> => {
    event.preventDefault();

    const loginUri = UriBuilder.use().Login().Build();
    try {
      const response = await httpRequestService.PostResourceAsync<IUserLogin>(loginUri, userLogin);
      console.log('Response value:', response);
    }
    catch (error) {
      console.log('Error fetching data:', error);
    }
  }

  const handleInputChange = (event: ChangeEvent<HTMLInputElement>): void => {
    const { name, value } = event.target;

    setUserLogin((prevState) => ({
      ...prevState,
      [name]: value
    }));
  };

  return (
    <div className='main-container'>
      <Title title={'Login'} />
      <br />
      <InputForm configurations={getLoginConfig({ onChange: handleInputChange, onClick: handleLogin, formDetails: userLogin })} />
    </div>
  )
}

export default Login;