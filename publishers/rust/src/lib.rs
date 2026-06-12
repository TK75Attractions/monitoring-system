use tracing::Subscriber;
use tracing_subscriber::Layer;

pub fn add(left: u64, right: u64) -> u64 {
    left + right
}

pub struct NatsLayer {}

impl<S> Layer<S> for NatsLayer where S: Subscriber {}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn it_works() {
        let result = add(2, 2);
        assert_eq!(result, 4);
    }
}
