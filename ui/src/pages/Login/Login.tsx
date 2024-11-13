import React, { ChangeEvent, MouseEvent, useState } from 'react'

// Our stuff
import './Login.css'
import Title from '../../components/title/Title';
import Button from '../../components/button/Button';
import { useHttpRequestService } from '../../contexts/HttpRequestServiceContext';
import UriBuilder from '../../utils/UriBuilder';
import { getLoginConfig, IUserLogin } from '../../configs/form-config';
import InputForm from '../../components/input-form/InputForm';
<<<<<<< HEAD
import { IHttpRequestService } from '../../services/interfaces/IHttpRequestService';
import Loader from '../../components/loader/Loader';
=======
>>>>>>> parent of d48865f (Implemented an easier way to create form inputs. Does not apply the Open/Close principle yet.)

const Login = (): JSX.Element => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [userLogin, setUserLogin] = useState<IUserLogin>({ email: '', password: '' });
  const httpRequestService = useHttpRequestService();

  const handleLogin = async (event: MouseEvent<HTMLInputElement>) => {
    event.preventDefault();
    setIsLoading(true);

    const loginUri = UriBuilder.use().Login().Build();
    try {
      const response = await httpRequestService.PostResourceAsync<IUserLogin>(loginUri, userLogin);
      console.log('Response value:', response);
    }
    catch (error) {
      console.log('Error fetching data:', error);
    }
    finally {
      setIsLoading(false);
    }
  }

  const handleInputChange = (event: ChangeEvent<HTMLInputElement>): void => {
    const { name, value } = event.target;

    setUserLogin((prevState) => ({
      ...prevState,
      [name]: value
    }));
  };

  const content =
    (
      <>
        <Title title={'Login'} />
        <br />
        <InputForm configurations={getLoginConfig({ onChange: handleInputChange, onClick: handleLogin, formDetails: userLogin })} />
      </>
    );

  return (
    <div className='main-container'>
      <Title title={'Login'} />
      <br />
      <InputForm configurations={getLoginConfig({ onChange: handleInputChange, formDetails: userLogin })} />
      <Button onHandleButtonClick={handleLogin} />
    </div>
  )
}

export default Login;