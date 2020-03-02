alter table usuarios 
add column siguiendo tinyint null default 0;

CREATE TABLE `chat_bot`.`seguidores` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `idUsuario` INT NOT NULL,
  `fecha` DATETIME NOT,
  `siguiendo` BIT(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id`),
  INDEX `seguidor_usuario_idx` (`idUsuario` ASC),
  CONSTRAINT `seguidor_usuario`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `chat_bot`.`usuarios` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
	 
alter table usuarios 
drop column siguiendo;

alter table usuarios 
CHANGE COLUMN nombre alias VARCHAR(100) NOT NULL ;
ALTER TABLE usuarios 
ADD COLUMN nombre VARCHAR(100) NULL;