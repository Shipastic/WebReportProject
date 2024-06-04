const BackgroundComponent = () => {
    const contentStyle = {
        backgroundImage: `url(${backgroundImage})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        height: '100vh', // Убедитесь, что высота достаточно большая, чтобы отображать картинку
    };
  return (
    <p>Hello world!</p>
  );
}

export default BackgroundComponent;